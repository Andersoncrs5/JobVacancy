using StackExchange.Redis;

namespace JobVacancy.API.Services.Interfaces;

public interface IRedisService
{
    Task<bool> CreateAsync<T>(RedisKey key, T value, TimeSpan? ttl = null);
    Task<T?> GetAsync<T>(RedisKey key);
    Task<bool> ExistsAsync(RedisKey key);
    Task<bool> DeleteAsync(RedisKey key);
    Task<bool> RefreshTtlAsync(RedisKey key, TimeSpan? ttl = null);
}