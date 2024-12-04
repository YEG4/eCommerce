using System.Text.Json;
using eCommerce.Core.Services;
using StackExchange.Redis;

namespace eCommerce.Service
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _db;

        public CacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan expireTime)
        {
            if (response is null) return;
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; // store response as camelcase
            var responseSerialized = JsonSerializer.Serialize(response, options);
            await _db.StringSetAsync(cacheKey, responseSerialized, expireTime);
        }

        public async Task<string?> GetCachedData(string cacheKey)
        {
            var response = await _db.StringGetAsync(cacheKey);
            if (response.IsNullOrEmpty) return null;
            return response;
        }
    }
}
