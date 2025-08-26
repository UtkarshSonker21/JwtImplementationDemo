using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace JwtImplementationDemo.Helper.Filters
{
    public class CachingFilter : IResourceFilter
    {
        private readonly IMemoryCache _cache;

        public CachingFilter(IMemoryCache cache)
        {
            _cache = cache;
        }


        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var key = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            if (_cache.TryGetValue(key, out var cachedResponse))
            {
                context.Result = cachedResponse as IActionResult;
            }
        }


        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            var key = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;

            if (!_cache.TryGetValue(key, out _))
            {
                _cache.Set(key, context.Result, TimeSpan.FromMinutes(5));
            }
        }

    }
}
