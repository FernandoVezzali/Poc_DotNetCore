using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc_Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class FlushCacheAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheKey = context.HttpContext.Request.Query["cacheKey"];

            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
            //var cacheOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<CacheOptions>>();

            //if (cacheService.TryGetValue(cacheKey, out object cachedData))
            //{
                //var executedContext = await next();

                //if (executedContext.Result is OkObjectResult okObjectResult)
                //{
                    //var timeToLiveSeconds = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(cacheOptions.Value.SlidingExpiration) };
                    //cacheService.Set(cacheKey, okObjectResult.Value, timeToLiveSeconds);
                //}
            //}

            cacheService.Remove(cacheKey.ToString());

            

            await next();

            return;
        }
    }
}
