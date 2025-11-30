using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Cashe
{
    public class RedisCacheService : ICashService
    {
        private readonly IConnectionMultiplexer _conn;
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer conn)
        {
            _conn = conn;
            _db = _conn.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var data = await _db.StringGetAsync(key);
            if (!data.HasValue) return default;
            return JsonSerializer.Deserialize<T>(data!);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, ttl ?? TimeSpan.FromMinutes(5));
        }


        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
