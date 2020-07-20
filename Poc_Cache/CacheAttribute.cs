using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Poc_Cache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _cacheKey;

        public CachedAttribute(string cacheKey)
        {
            _cacheKey = cacheKey;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                var cacheOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<CacheOptions>>();

                if (!cacheService.TryGetValue(_cacheKey, out object cachedData))
                {
                    var executedContext = await next();

                    if (executedContext.Result is OkObjectResult okObjectResult)
                    {
                        var timeToLiveSeconds = new MemoryCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(cacheOptions.Value.Expiration) };
                        cacheService.Set(_cacheKey, okObjectResult.Value, timeToLiveSeconds);
                    }
                }

                if (cachedData != null)
                {
                    var contentResult = new ContentResult
                    {
                        Content = JsonSerializer.Serialize(cachedData),
                        ContentType = "application/json",
                        StatusCode = 200
                    };
                    context.Result = contentResult;
                    return;
                }
            }
            catch
            {
                await next();
                return;
            }
        }
    }
}