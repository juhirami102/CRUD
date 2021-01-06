using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Go2Share.Web.Helper
{
    public class RedisCacheHelper
    {
        private readonly IDistributedCache _cache;

        public RedisCacheHelper(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<bool> Exists(string key)
         => await Get(key) != null;

        public async Task<string> Get(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task Set(string key, string value)
        => await _cache.SetStringAsync(key,
                value, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = null
                });

        public void Remove(string key)
        {
            _cache.RemoveAsync(key);
        }
    }
}
