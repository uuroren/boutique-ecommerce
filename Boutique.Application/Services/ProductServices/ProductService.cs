using AutoMapper;
using Boutique.Application.Dtos;
using Boutique.Application.Dtos.ProductDtos;
using Boutique.Application.Services.ProductServices;
using Boutique.Application.Services.SearchServices;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using Boutique.Infrastructure.Repositories.ProductRepositories;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading.Tasks;
using static Boutique.Domain.Entities.Product;

namespace Boutique.Application.Services {
    public class ProductService:IProductService {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly RedisCacheService _cacheService;
        private readonly AwsS3Service _awsS3Service;
        private readonly IElasticsearchService _elasticsearchService;
        public ProductService(IProductRepository productRepository,IMapper mapper,RedisCacheService cacheService,AwsS3Service awsS3Service,IElasticsearchService elasticsearchService) {
            _productRepository = productRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _awsS3Service = awsS3Service;
            _elasticsearchService = elasticsearchService;
        }

        public async Task CreateProductAsync(CreateProductDto productDto) {
            var product = _mapper.Map<Product>(productDto);
            product.CreatedAt = DateTime.UtcNow.AddHours(3);
            product.UpdatedAt = DateTime.UtcNow.AddHours(3);

            product.ImageUrls = new List<string>();

            if(productDto.Images != null && productDto.Images.Count > 0) {
                foreach(var image in productDto.Images) {
                    using(var stream = image.OpenReadStream()) {
                        var imageUrl = await _awsS3Service.UploadFileAsync(stream,image.FileName,image.ContentType);
                        product.ImageUrls.Add(imageUrl);
                    }
                }
            }

            if(!string.IsNullOrEmpty(productDto.Variants)) {
                var variants = JsonConvert.DeserializeObject<List<ProductVariantDto>>(productDto.Variants.Trim());
                product.Variants = _mapper.Map<List<ProductVariant>>(variants);
            }

            if(!string.IsNullOrEmpty(productDto.Tags)) {
                var tags = productDto.Tags.Split(",");
                product.Tags = tags.Select(t => t.Trim()).ToList();
            }

            await _productRepository.CreateProductAsync(product);
            await _elasticsearchService.IndexProductAsync(product);

            var redisKeyExisting = await _cacheService.KeyExistsAsync("products");
            if(redisKeyExisting) {
                await _cacheService.RemoveCacheValueAsync("products");
            }
        }

        public async Task DeleteProductAsync(string productId) {
            await _productRepository.DeleteProductAsync(productId);
            await _elasticsearchService.DeleteProductFromIndexAsync(productId);
        }

        public async Task<ResultProductDto> GetProductByIdAsync(string productId) {
            var product = await _productRepository.GetProductByIdAsync(productId);
            var productDto = _mapper.Map<ResultProductDto>(product);
            return productDto;
        }

        public async Task<ResultProductDto> GetProductByProductCodeAsync(string productCode) {
            var product = await _productRepository.GetProductByProductCodeAsync(productCode);
            var productDto = _mapper.Map<ResultProductDto>(product);
            return productDto;
        }

        public async Task<List<ResultProductDto>> GetProductsAsync() {
            var cacheKey = "products";
            var cachedProducts = await _cacheService.GetCacheValueAsync<List<ResultProductDto>>(cacheKey);
            if(cachedProducts != null && cachedProducts.Count > 0) {
                return cachedProducts;
            }

            var products = await _productRepository.GetProductsAsync();
            var productDtos = _mapper.Map<List<ResultProductDto>>(products);

            await _cacheService.SetCacheValueAsync(cacheKey,productDtos,TimeSpan.FromHours(3));
            return productDtos;
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto) {
            var product = await _productRepository.GetProductByIdAsync(productDto.ProductId);
            if(product == null) {
                throw new KeyNotFoundException("Product not found");
            }

            // Update basic product information
            product.Name = productDto.Name;
            product.Description = productDto.Description;
            product.CategoryId = productDto.CategoryId;

            // Update price and calculate discount if necessary
            if(product.Price != productDto.NewPrice) {
                var oldPrice = product.Price;
                var newPrice = productDto.NewPrice;
                product.NewPrice = newPrice;
                product.DiscountPercentage = ((oldPrice - newPrice) / oldPrice) * 100;
            }

            // Update image URLs if provided
            if(productDto.ExistingImageUrls != null) {
                product.ImageUrls = productDto.ExistingImageUrls;
            }

            // Add new images if provided
            if(productDto.NewImages != null && productDto.NewImages.Count > 0) {
                foreach(var image in productDto.NewImages) {
                    using(var stream = image.OpenReadStream()) {
                        var imageUrl = await _awsS3Service.UploadFileAsync(stream,image.FileName,image.ContentType);
                        product.ImageUrls.Add(imageUrl);
                    }
                }
            }

            // Update product variants
            if(!string.IsNullOrEmpty(productDto.Variants)) {
                var variants = JsonConvert.DeserializeObject<List<ProductVariantDto>>(productDto.Variants);
                product.Variants = _mapper.Map<List<ProductVariant>>(variants);
            }

            product.UpdatedAt = DateTime.UtcNow.AddHours(3);
            await _productRepository.UpdateProductAsync(product);
            await _elasticsearchService.IndexProductAsync(product);

            await _cacheService.SetCacheValueAsync("products",product,TimeSpan.FromHours(3));
        }
    }
}
