using Boutique.Application.Dtos;
using Boutique.Application.Dtos.ProductDtos;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.ProductServices {
    public interface IProductService {
        Task CreateProductAsync(CreateProductDto productDto);
        Task DeleteProductAsync(string productId);
        Task<ResultProductDto> GetProductByIdAsync(string productId);
        Task<ResultProductDto> GetProductByProductCodeAsync(string productCode);
        Task<List<ResultProductDto>> GetProductsAsync();
        Task UpdateProductAsync(UpdateProductDto productDto);
    }
}
