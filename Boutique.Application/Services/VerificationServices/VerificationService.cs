using global::Boutique.Infrastructure.ExternalServices;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace Boutique.Application.Services {
    public class VerificationService {
        private readonly RedisCacheService redisCacheService;
        public VerificationService(RedisCacheService redis) {
            redisCacheService = redis;
        }

        public async Task StoreVerificationCodeAsync(string phoneNumber,string code) {
            await redisCacheService.SetCacheValueAsync(GetRedisKey(phoneNumber),code,TimeSpan.FromMinutes(2));
        }

        public async Task<bool> ValidateVerificationCodeAsync(string phoneNumber,string code) {
            var storedCode = await redisCacheService.GetCachValueAsync(GetRedisKey(phoneNumber));
            if(string.IsNullOrEmpty(storedCode)) {
                return false;
            }

            // Doğrulama kodunu kontrol et
            var cleanedStoredCode = storedCode.ToString().Trim('"');

            if(cleanedStoredCode == code) {
                // Kodu doğrulandıktan sonra sil
                await redisCacheService.RemoveCacheValueAsync(GetRedisKey(phoneNumber));
                return true;
            }

            return false;
        }

        private string GetRedisKey(string phoneNumber) {
            return $"verification-code:{phoneNumber}";
        }

        public async Task<bool> IsCodeExpiredAsync(string phoneNumber) {
            return !await redisCacheService.KeyExistsAsync(phoneNumber);
        }
    }

}
