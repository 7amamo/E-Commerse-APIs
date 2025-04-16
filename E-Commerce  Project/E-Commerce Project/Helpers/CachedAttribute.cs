using Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce_Project.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int expireTimeInSeconds;

        public CachedAttribute(int ExpireTimeInSeconds)
        {
            expireTimeInSeconds = ExpireTimeInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            string cacheKey = GeneratrCacheKeyFromRequest(context.HttpContext.Request);

            var CachedResponse = await cacheService.GetCashedResponse(cacheKey);

            if (! string.IsNullOrEmpty(CachedResponse))
            {
                var contentResuly = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResuly;
                return;
            }
            var Data = await next.Invoke();

            if (Data.Result is OkObjectResult data)
            {
                await cacheService.CacheResponseAsync(cacheKey, data.Value, TimeSpan.FromSeconds(expireTimeInSeconds));
            }


        }

        private string GeneratrCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);  // api/ Products

            foreach (var (key ,value ) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
