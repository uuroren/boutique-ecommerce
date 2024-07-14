using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boutique.Infrastructure.ExternalServices {
    public class RedisCacheService {
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IConnectionMultiplexer redis) {
            _redis = redis;

        }

        public async Task SetCacheValueAsync<T>(string key,T value,TimeSpan? expiry = null) {
            var db = _redis.GetDatabase();
            var serializedValue = JsonConvert.SerializeObject(value);
            await db.StringSetAsync(key,serializedValue,expiry);
        }

        public async Task<T> GetCacheValueAsync<T>(string key) {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            return value.HasValue ? JsonConvert.DeserializeObject<T>(value) : default;
        }

        public async Task<string> GetCachValueAsync(string key) {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);
            return value;
        }

        public async Task RemoveCacheValueAsync(string key) {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(key);
        }

        public async Task<bool> KeyExistsAsync(string phoneNumber) {
            var db = _redis.GetDatabase();
            var result = !await db.KeyExistsAsync(phoneNumber);
            return result;
        }
    }
}
