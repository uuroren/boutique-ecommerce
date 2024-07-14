using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.Repositories.FavoriteRepositories {
    public interface IFavoriteRepository {
        Task AddFavoriteAsync(Favorite favorite);
        Task<IEnumerable<Favorite>> GetUserFavoritesAsync(string userId);
        Task RemoveFavoriteAsync(string userId,string productId);
    }
}
