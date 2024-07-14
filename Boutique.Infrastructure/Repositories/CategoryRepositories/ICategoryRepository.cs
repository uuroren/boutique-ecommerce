using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CategoryRepositories
{
    public interface ICategoryRepository
    {
        Task CreateCategoryAsync(Category category);
        Task DeleteCategoryAsync(string id);
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(string id);
        Task UpdateCategoryAsync(Category category);
    }
}
