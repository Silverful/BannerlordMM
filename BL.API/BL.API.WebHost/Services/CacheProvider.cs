using BL.API.Core.Abstractions.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace BL.API.WebHost.Services
{
    public class CacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private object _lock = new object();

        public CacheProvider(IMemoryCache cache,
            IConfiguration configuration)
        {
            _cache = cache;
            _configuration = configuration;
        }

        public bool TryGetValue(object key, out object value)
        {
            return _cache.TryGetValue(key, out value);
        }

        public object GetCachedResponse(object key, Func<Task<object>> getDataFunc)
        {
            object value;

            if (!_cache.TryGetValue(key, out value))
            {
                lock (_lock)
                {
                    if (!_cache.TryGetValue(key, out value))
                    {
                        var data = getDataFunc().GetAwaiter().GetResult();
                        value = data;

                        var options = new MemoryCacheEntryOptions()
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(Convert.ToInt32(_configuration["Cache:AbsoluteExpiration"])),
                            SlidingExpiration = TimeSpan.FromMinutes(Convert.ToInt32(_configuration["Cache:SlidingExpiration"]))
                        };
                        _cache.Set(key, value, options);
                    }

                    return value;
                }
            }

            return value;
        }

        public bool TryRemoveValue(object key)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
