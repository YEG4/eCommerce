﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerce.Core.Services
{
    public interface ICacheService
    {
        public Task CacheResponseAsync(string cacheKey, object response, TimeSpan expireTime);

        public Task<string?> GetCachedData(string cacheKey);
    }
}
