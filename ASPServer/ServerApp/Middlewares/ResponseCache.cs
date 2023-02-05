using System.Text;
using Microsoft.Extensions.Caching.Memory;
using ServerApp.Services;

namespace ServerApp.Middlewares;

public class ResponseCache
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ResponseCache> _logger;
    
    public ResponseCache(
        RequestDelegate next,
        IMemoryCache memoryCache,
        ILogger<ResponseCache> logger)
    {
        _next = next;
        _memoryCache = memoryCache;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext httpContext, RequestContext requestContext)
    {
        var cacheKey = requestContext.CacheKey;
        
        if (_memoryCache.TryGetValue(cacheKey, out string cachedResponse))
        {
            _logger.LogInformation("[ResponseCache] FromCache: {cachedResponse}", cachedResponse);
            await httpContext.Response.WriteAsync(cachedResponse);
            return;
        }
        
        var originalStream = httpContext.Response.Body;

        try
        {
            await using var ms = new MemoryStream();
            httpContext.Response.Body = ms;
            
            await _next.Invoke(httpContext);

            var response = Encoding.UTF8.GetString(ms.ToArray());
            _logger.LogInformation("[ResponseCache] FromController: {response}", response);
            _memoryCache.Set(cacheKey, response, TimeSpan.FromMinutes(1));
            
            ms.Position = 0;
            await ms.CopyToAsync(originalStream);
        }
        finally
        {
            httpContext.Response.Body = originalStream;
        }
    }
}