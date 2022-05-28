using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketTracking.Utility
{
    public class CacheHelper
    {

        private IDistributedCache _cache;

        public CacheHelper(IDistributedCache cache)
        {
            _cache = cache;
        }

        public string GetStrCache(string key)
        {
            return _cache.GetString(key);
        }
        public void SetStrCache(string key, string value, double milliSeconds = 1200)
        {
            // Keep in cache for this time, reset time if accessed.
            double sliding = milliSeconds;

            var sliding2 = TimeSpan.FromSeconds(sliding);
            var opt = new DistributedCacheEntryOptions().SetSlidingExpiration(sliding2);

            lock (_cache)
            {
                _cache.SetString(key, value, opt);
            }
        }
    }
}
