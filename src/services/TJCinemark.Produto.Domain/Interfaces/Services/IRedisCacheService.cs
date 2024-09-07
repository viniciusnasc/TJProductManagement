namespace TJ.ProductManagement.Domain.Interfaces.Services
{
    public interface IRedisCacheService
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task RemoveAsync(string key);
        long GetCount<T>();
        Task SetAll<T>(IEnumerable<T> items);
        IEnumerable<T> GetAll<T>();
    }
}
