using Boutique.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data {
    public class MongoDbContext {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration) {
            var client = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
            _database = client.GetDatabase("BoutiqueDb");
        }

        public IMongoCollection<Favorite> Favorites => _database.GetCollection<Favorite>("Favorites");
        public IMongoCollection<Cart> Carts => _database.GetCollection<Cart>("Carts");
        public IMongoCollection<Comment> Comments => _database.GetCollection<Comment>("Comments");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
        public IMongoCollection<Order> Orders => _database.GetCollection<Order>("Orders");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Payment> Payments => _database.GetCollection<Payment>("Payments");
        public IMongoCollection<PaymentTransaction> PaymentErrors => _database.GetCollection<PaymentTransaction>("PaymentErrors");
    }
}
