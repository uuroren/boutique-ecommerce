using Boutique.Application.Interfaces;
using Boutique.Infrastructure.ExternalServices;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Application.Services.RefreshTokenServices {
    public class RefreshTokenService {
        private readonly RedisCacheService _redis;
        public RefreshTokenService(RedisCacheService redis) {
            _redis = redis;
        }

        public async Task StoreRefreshTokenAsync(string userId,string refreshToken) {

            await _redis.SetCacheValueAsync(userId,refreshToken,TimeSpan.FromDays(1));
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId,string refreshToken) {
            var storedToken = await _redis.GetCachValueAsync(userId);
            return storedToken == refreshToken;
        }

        public async Task<string> GetRefreshTokenAsync(string userId) {
            return await _redis.GetCachValueAsync(userId);
        }
    }
}
