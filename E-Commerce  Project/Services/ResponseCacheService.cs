using Core.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _redis;

        public ResponseCacheService(IConnectionMultiplexer redis )
        {
            _redis = redis.GetDatabase();
        }

        public  async Task CacheResponseAsync(string CacheKey, object Response, TimeSpan ExpireTime)
        {
            if (Response == null) return;

            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var SerializeResponse = JsonSerializer.Serialize(Response , options);

            _redis.StringSetAsync(CacheKey, SerializeResponse, ExpireTime);
   
        }

        public async Task<string?> GetCashedResponse(string CacheKey)
        {
            var cachedResponse =await _redis.StringGetAsync(CacheKey);
            if (cachedResponse.IsNullOrEmpty) return null;
            return cachedResponse;
        }
    }
}
