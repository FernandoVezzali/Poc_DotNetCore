using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
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

            cacheService.Remove(cacheKey.ToString());

            await next();

            return;
        }
    }
}
