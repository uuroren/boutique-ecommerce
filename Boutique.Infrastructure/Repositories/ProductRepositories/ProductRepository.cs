using Boutique.Domain.Entities;
using Boutique.Infrastructure.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.ProductRepositories {
    public class ProductRepository:IProductRepository {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbContext context) {
            _products = context.Products;
        }
        public async Task CreateProductAsync(Product product) {
            var productCode = await GenerateUniqueProductCodeAsync();

            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            product.ProductCode = productCode;

            await _products.InsertOneAsync(product);
        }
        public async Task DeleteProductAsync(string productId) {
            var filter = Builders<Product>.Filter.Eq(c => c.ProductId,productId);
            await _products.DeleteOneAsync(filter);
        }

        public async Task<Product> GetProductByIdAsync(string productId) {
            var filter = Builders<Product>.Filter.Eq(c => c.ProductId,productId);
            return await _products.Find(filter).FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Product>> GetProductsAsync() {
            return await _products.Find(Builders<Product>.Filter.Empty).ToListAsync();
        }

        public async Task UpdateProductAsync(Product product) {
            var filter = Builders<Product>.Filter.Eq(c => c.ProductId,product.ProductId);
            product.UpdatedAt = DateTime.UtcNow;

            await _products.ReplaceOneAsync(filter,product);
        }

        public async Task<string> GenerateUniqueProductCodeAsync() {
            var existingCodes = await _products.Find(_ => true)
                                                        .Project(p => p.ProductCode)
                                                        .ToListAsync();
            string newCode;
            var random = new Random();

            do {
                newCode = "PRD-" + random.Next(1000,9999);
            } while(existingCodes.Contains(newCode));

            return newCode;
        }

        public async Task<Product> GetProductByProductCodeAsync(string productCode) {
            var filter = Builders<Product>.Filter.Eq(c => c.ProductCode,productCode);
            return await _products.Find(filter).FirstOrDefaultAsync();
        }
    }
}
