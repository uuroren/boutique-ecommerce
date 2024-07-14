using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    using Boutique.API.Models;
    using Boutique.Application.Dtos;
    using Boutique.Application.Dtos.ProductDtos;
    using Boutique.Application.Services;
    using Boutique.Application.Services.ProductServices;
    using Boutique.Application.Services.SearchServices;
    using Boutique.Domain.Entities;
    using Boutique.Infrastructure.ExternalServices;
    using Boutique.Infrastructure.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController:ControllerBase {
        private readonly IProductService _productService;
        private readonly IElasticsearchService _elasticsearchService;
        public ProductsController(IProductService productService,IElasticsearchService elasticsearchService) {
            _productService = productService;
            _elasticsearchService = elasticsearchService;
        }

        [Authorize]
        [HttpGet("get-all-products")]
        public async Task<ActionResult<IEnumerable<ResultProductDto>>> GetAllProducts() {
            var products = await _productService.GetProductsAsync();
            return Ok(new BaseResponse { Result = products,Success = true });
        }

        [Authorize]
        [HttpGet("get-product/{id}")]
        public async Task<ActionResult<ResultProductDto>> GetProductById(string id) {
            var product = await _productService.GetProductByIdAsync(id);
            if(product == null) {
                return NotFound(new BaseResponse { Success = false,Message = "No product matching the submitted ID was found" });
            }

            return Ok(new BaseResponse { Result = product,Success = true });
        }

        [Authorize]
        [HttpGet("get-product-code/{productCode}")]
        public async Task<ActionResult<ResultProductDto>> GetProductByCode(string productCode) {
            var product = await _productService.GetProductByProductCodeAsync(productCode);
            if(product == null) {
                return NotFound(new BaseResponse { Success = false,Message = "No product matching the submitted ID was found" });
            }

            return Ok(new BaseResponse { Result = product,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("create-product")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductDto productDto) {
            await _productService.CreateProductAsync(productDto);
            return Ok(new BaseResponse { Message = "The product has been added successfully!",Result = productDto,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-product")]
        public async Task<IActionResult> UpdateProduct([FromForm] UpdateProductDto productDto) {
            await _productService.UpdateProductAsync(productDto);
            return Ok(new BaseResponse { Message = "The product has been updated successfully!",Result = productDto,Success = true });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(string id) {
            await _productService.DeleteProductAsync(id);
            return Ok(new BaseResponse { Message = "The item was successfully deleted!",Success = true });
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Product>>> SearchProducts([FromQuery] string query) {
            var products = await _elasticsearchService.SearchProductsAsync(query);

            if(products == null || !products.Any()) {
                return NotFound(new BaseResponse { Message = "No products found." });
            }

            return Ok(new BaseResponse { Result = products,Success = true });
        }

        [HttpGet("categories/search")]
        public async Task<ActionResult<IEnumerable<Category>>> SearchCategories([FromQuery] string query) {
            var categories = await _elasticsearchService.SearchCategoriesAsync(query);
            return Ok(new BaseResponse { Result = categories,Success = true });
        }
    }
}

