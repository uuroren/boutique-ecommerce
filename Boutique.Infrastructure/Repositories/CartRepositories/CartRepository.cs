using Boutique.Domain.Entities;
using Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.CartRepositories {
    public class CartRepository:ICartRepository {
        private readonly IMongoCollection<Cart> _carts;

        public CartRepository(MongoDbContext context) {
            _carts = context.Carts;
        }

        public async Task CreateCartAsync(Cart cart) {
            await _carts.InsertOneAsync(cart);
        }

        public async Task UpdateCartAsync(Cart cart) {
            var filter = Builders<Cart>.Filter.Eq(c => c.Id,cart.Id);
            await _carts.ReplaceOneAsync(filter,cart);
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId) {
            var filter = Builders<Cart>.Filter.Eq(c => c.UserId,userId);
            return await _carts.Find(filter).FirstOrDefaultAsync();
        }

        public async Task RemoveCartItemAsync(string userId,string cartItemId) {
            var filter = Builders<Cart>.Filter.And(
                Builders<Cart>.Filter.Eq(c => c.UserId,userId),
                Builders<Cart>.Filter.ElemMatch(c => c.Items,i => i.ProductId == cartItemId)
            );

            var update = Builders<Cart>.Update.PullFilter(c => c.Items,i => i.ProductId == cartItemId);

            await _carts.UpdateOneAsync(filter,update);
        }

        public async Task<Cart> GetCartByIdAsync(string cartId) {
            var filter = Builders<Cart>.Filter.Eq(c => c.Id,cartId);
            return await _carts.Find(filter).FirstOrDefaultAsync();
        }
    }
}
