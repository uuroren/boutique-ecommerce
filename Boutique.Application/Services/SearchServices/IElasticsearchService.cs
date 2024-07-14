using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.SearchServices {
    public interface IElasticsearchService {
        Task IndexProductAsync(Product product);
        Task IndexCategoryAsync(Category category);
        Task<IEnumerable<Product>> SearchProductsAsync(string query);
        Task<IEnumerable<Category>> SearchCategoriesAsync(string query);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task DeleteProductFromIndexAsync(string productId);
    }
}
