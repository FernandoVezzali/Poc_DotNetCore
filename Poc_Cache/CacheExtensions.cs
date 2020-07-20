using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc_Cache
{
    public static class CacheExtensions
    {
        public static void AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheOptions = configuration.GetSection("Cache").Get<CacheOptions>();

            services.Configure<CacheOptions>(config =>
            {
                config.Expiration = cacheOptions.Expiration;
            });
        }
    }
}
