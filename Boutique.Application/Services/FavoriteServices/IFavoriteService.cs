using Boutique.Application.Dtos.FavoriteDtos;
using Boutique.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.FavoriteServices {
    public interface IFavoriteService {
        Task<bool> AddFavoriteAsync(Favorite favorite);
        Task<IEnumerable<ResultFavoriteDto>> GetUserFavoritesAsync(string userId);
        Task<bool> RemoveFavoriteAsync(string userId,string productId);
    }
}
