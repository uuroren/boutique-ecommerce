using Boutique.Domain.Entities;
using Infrastructure.Data;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.FavoriteRepositories {
    // src/Infrastructure/Data/MongoDB/FavoriteRepository.cs
    public class FavoriteRepository:IFavoriteRepository {
        private readonly IMongoCollection<Favorite> _favorites;

        public FavoriteRepository(MongoDbContext context) {
            _favorites = context.Favorites;
        }
        public async Task AddFavoriteAsync(Favorite favorite) {
            favorite.CreatedAt = DateTime.Now;
            await _favorites.InsertOneAsync(favorite);
        }

        public async Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId) {
            return await _favorites.Find(f => f.UserId == userId).ToListAsync();
        }

        public async Task RemoveFavoriteAsync(string userId,string productId) {
            await _favorites.DeleteOneAsync(f => f.UserId == userId && f.ProductId == productId);
        }
    }

}
