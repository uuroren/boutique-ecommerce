using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.ProductRepositories {
    public interface IProductRepository {
        Task CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(string productId);
        Task<Product> GetProductByProductCodeAsync(string productCode);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(string productId);
    }
}
