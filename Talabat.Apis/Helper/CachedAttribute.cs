using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.Apis.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSec;

        public CachedAttribute(int ExpireTimeInSec)
        {
            _expireTimeInSec = ExpireTimeInSec;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CachedService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var CacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var CacheResponse = await CachedService.GetCachedResponse(CacheKey);
            if (!string.IsNullOrEmpty(CacheResponse))
            {
                var ContentResult = new ContentResult()
                {
                    Content = CacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = ContentResult;
                return;
            }
            var ExecutedEndPointContext = await next.Invoke();
            if (ExecutedEndPointContext.Result is OkObjectResult result)
            {
                await CachedService.CacheResponseAsync(CacheKey,result.Value,TimeSpan.FromSeconds(_expireTimeInSec));
            }
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request) {
            var KeyBuilder =  new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key)) 
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
