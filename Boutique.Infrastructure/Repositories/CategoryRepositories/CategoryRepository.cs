using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Boutique.Infrastructure.ExternalServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CategoryRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categories;
        public CategoryRepository(MongoDbContext context)
        {
            _categories = context.Categories;
        }

        public async Task CreateCategoryAsync(Category category)
        {
            category.CreatedAt = DateTime.Now.AddHours(3);
            category.UpdatedAt = DateTime.Now.AddHours(3);

            await _categories.InsertOneAsync(category);
        }

        public async Task DeleteCategoryAsync(string id)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.CategoryId, id);
            await _categories.DeleteOneAsync(filter);
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categories.Find(Builders<Category>.Filter.Empty).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.CategoryId, id);
            return await _categories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.CategoryId, category.CategoryId);
            category.UpdatedAt = DateTime.Now.AddHours(3);

            await _categories.ReplaceOneAsync(filter, category);
        }
    }
}
