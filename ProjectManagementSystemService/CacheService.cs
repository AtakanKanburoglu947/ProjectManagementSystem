using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementSystemService
{
    public class CacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public MemoryCacheEntryOptions GetMemoryCacheEntryOptions(TimeSpan absoluteExpiration, TimeSpan slidingExpiration)
        {
            return new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpiration,
                SlidingExpiration = slidingExpiration
            };
        }
        public async Task<T> Get<T>(string cacheKey, TimeSpan absoluteExpiration, TimeSpan slidingExpiration, Func<Task<T>> GetData) where T : class
        {
            if (_memoryCache.TryGetValue(cacheKey, out T? cachedData))
            {
                return cachedData!;
            }
            T data = await GetData();
            if (data != null)
            {
                MemoryCacheEntryOptions memoryCacheEntryOptions = GetMemoryCacheEntryOptions(absoluteExpiration, slidingExpiration);
                _memoryCache.Set(cacheKey, data, memoryCacheEntryOptions);
                
                return data;
            }
            return null;

        }
        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
