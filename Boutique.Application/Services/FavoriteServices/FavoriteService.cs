using AutoMapper;
using Boutique.Application.Dtos.FavoriteDtos;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using Boutique.Infrastructure.Repositories.FavoriteRepositories;
using Boutique.Infrastructure.Repositories.ProductRepositories;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.FavoriteServices {
    // src/Application/Services/FavoriteService.cs
    public class FavoriteService:IFavoriteService {
        private readonly IFavoriteRepository _favoriteRepository;
        private IMapper _mapper;
        private IProductRepository _productRepository;
        private RedisCacheService _redis;
        private UserManager<User> _userManager;
        public FavoriteService(IFavoriteRepository favoriteRepository,
            IMapper mapper,IProductRepository productRepository,
            RedisCacheService redis,
            UserManager<User> userManager) {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
            _productRepository = productRepository;
            _redis = redis;
            _userManager = userManager;
        }

        public async Task<bool> AddFavoriteAsync(Favorite favorite) {
            var user = await _userManager.FindByIdAsync(favorite.Id);
            if(user == null) {
                return false;
            }

            favorite.CreatedAt = DateTime.UtcNow;
            await _favoriteRepository.AddFavoriteAsync(favorite);

            var redisKeyExisting = await _redis.KeyExistsAsync($"favorites_{favorite.UserId}");
            if(redisKeyExisting) {
                await _redis.RemoveCacheValueAsync($"favorites_{favorite.UserId}");
            }

            return true;
        }

        public async Task<IEnumerable<ResultFavoriteDto>> GetUserFavoritesAsync(string userId) {
            var cacheValue = await _redis.GetCacheValueAsync<List<ResultFavoriteDto>>($"favorites_{userId}");
            if(cacheValue != null && cacheValue.Count > 0) {
                return cacheValue;
            }

            var favorites = await _favoriteRepository.GetUserFavoritesAsync(userId);

            var tasks = favorites.Select(async favorite => {
                var product = await _productRepository.GetProductByIdAsync(favorite.ProductId);
                if(product != null) {
                    return new ResultFavoriteDto {
                        UserId = favorite.ProductId,
                        Product = product,
                    };
                }
                return null;
            });

            var resultFavorites = await Task.WhenAll(tasks);
            await _redis.SetCacheValueAsync($"favorites_{userId}",resultFavorites,TimeSpan.FromHours(3));

            return resultFavorites.Where(f => f != null);
        }

        public async Task<bool> RemoveFavoriteAsync(string userId,string productId) {
            await _favoriteRepository.RemoveFavoriteAsync(userId,productId);
            var redisKeyExisting = await _redis.KeyExistsAsync($"favorites_{userId}");
            if(redisKeyExisting) {
                await _redis.RemoveCacheValueAsync($"favorites_{userId}");
            }
            return true;
        }
    }
}
