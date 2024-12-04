using System.Text;
using eCommerce.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace eCommerce.API.Helpers
{
    public class CachedAttribute(int expireTimeInSeconds) : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSeconds = expireTimeInSeconds;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
           var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            var cachedKey = GenerateCachedKey(context.HttpContext.Request);
            var response = await cacheService.GetCachedData(cachedKey);
            if(!string.IsNullOrEmpty(response))
            {
                var contentResult = new ContentResult()
                { 
                    Content = response,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }
            var ExecutedContext =  await next.Invoke();
            if(ExecutedContext.Result is OkObjectResult result)
            {
                await cacheService.CacheResponseAsync(cachedKey, result, TimeSpan.FromSeconds(_expireTimeInSeconds));
            }
            
        }

        private string GenerateCachedKey(HttpRequest request)
        {
            var cachedKey = new StringBuilder();
            cachedKey.Append(request.Path); // api/[controller]
            foreach(var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                cachedKey.Append($"|{key}={value}");
            }
            return cachedKey.ToString();
        }
    }
}
