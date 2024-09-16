using Microsoft.Extensions.Caching.Memory;
using ProjectManagementSystemCore.Models;
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
        public async Task<T> Get<T>(string cacheKey,Guid userId, TimeSpan absoluteExpiration, TimeSpan slidingExpiration, Func<Task<T>> GetData) where T : class
        {
            if (_memoryCache.TryGetValue($"{cacheKey}_{userId}", out T? cachedData))
            {
                return cachedData!;
            }
            T data = await GetData();
            if (data != null)
            {
                SetClass(cacheKey,userId, data,absoluteExpiration,slidingExpiration);
                
                return data;
            }
            return null;

        }
        public async Task<T> Get<T>(string cacheKey,Guid userId, Func<Task<T>> GetData) where T : class
        {
            if (_memoryCache.TryGetValue($"{cacheKey}_{userId}", out T? cachedData))
            {
                return cachedData!;
            }
            T data = await GetData();
            if (data != null)
            {
                return data;
            }
            return null;
        }
        public T? Get<T>(string cacheKey,Guid userId, TimeSpan absoluteExpiration, TimeSpan slidingExpiration, Func<T> GetData) where T : struct
        {
            if (_memoryCache.TryGetValue($"{cacheKey}_{userId}", out T? cachedData))
            {
                return cachedData!;
            }
            T? data =  GetData();
            if (data != null)
            {
                
                SetStruct(cacheKey,userId, (T)data, absoluteExpiration, slidingExpiration);
                return data;
            }
            return null;

        }
        public T Get<T>(string cacheKey,Guid userId, T data) where T : class
        {
            if (_memoryCache.TryGetValue($"{cacheKey}_{userId}", out T cachedData))
            {
                return cachedData;
            }
            if (data != null)
            {

                return data;
            }
            return null;

        }
        public void Remove(string cacheKey,Guid userId)
        {
            _memoryCache.Remove($"{cacheKey}_{userId}");
        }
        public void SetClass<T>(string cacheKey,Guid userId, T data, TimeSpan absoluteExpiration, TimeSpan slidingExpiration) where T : class
        {
            MemoryCacheEntryOptions memoryCacheEntryOptions = GetMemoryCacheEntryOptions(absoluteExpiration, slidingExpiration);
            _memoryCache.Set($"{cacheKey}_{userId}", data, memoryCacheEntryOptions);
        }
        public void SetStruct<T>(string cacheKey,Guid userId, T data, TimeSpan absoluteExpiration, TimeSpan slidingExpiration) where T : struct
        {
            MemoryCacheEntryOptions memoryCacheEntryOptions = GetMemoryCacheEntryOptions(absoluteExpiration, slidingExpiration);
            _memoryCache.Set($"{cacheKey}_{userId}", data, memoryCacheEntryOptions);
        }
    }
}
