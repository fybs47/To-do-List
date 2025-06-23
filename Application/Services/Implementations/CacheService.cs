using Application.Services.Interfaces;

namespace Application.Services.Implementations;

public class MemoryCacheService  : ICacheService
{
    public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string key)
    {
        throw new NotImplementedException();
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        throw new NotImplementedException();
    }
}