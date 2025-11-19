using JobVacancy.API.Services.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace JobVacancy.API.Services.Providers;

public class RedisService(IDatabase db): IRedisService
{
    private readonly TimeSpan _defaultTtl = TimeSpan.FromMinutes(5);
    
    public async Task<bool> CreateAsync<T>(RedisKey key, T value, TimeSpan? ttl = null)
    {
        RedisValue json = (RedisValue) JsonSerializer.Serialize(value);
        return await db.StringSetAsync(key, json, ttl ?? _defaultTtl);
    }

    public async Task<T?> GetAsync<T>(RedisKey key)
    {
        RedisValue json = await db.StringGetAsync(key);
        if (json.IsNullOrEmpty) return default;

        return JsonSerializer.Deserialize<T>(json!);
    }
    
    public Task<bool> ExistsAsync(RedisKey key)
        => db.KeyExistsAsync(key);

    public Task<bool> DeleteAsync(RedisKey key)
        => db.KeyDeleteAsync(key);

    public Task<bool> RefreshTtlAsync(RedisKey key, TimeSpan? ttl = null)
        => db.KeyExpireAsync(key, ttl ?? _defaultTtl);
}