using System.Text;
using ServerApp.Services;
using StackExchange.Redis;

namespace ServerApp.Middlewares;

public class ResponseCache
{
    private readonly RequestDelegate _next;
    private readonly IDatabase _redis;
    private readonly ILogger<ResponseCache> _logger;
    
    public ResponseCache(
        RequestDelegate next,
        IDatabase redis,
        ILogger<ResponseCache> logger)
    {
        _next = next;
        _redis = redis;
        _logger = logger;
    }
    
    public async Task Invoke(HttpContext httpContext, RequestContext requestContext)
    {
        var cacheKey = requestContext.CacheKey;
        var cached = _redis.StringGet(cacheKey);
        if (cached.HasValue)
        {
            var strValue = cached.ToString();
            _logger.LogInformation("[ResponseCache] FromCache: {cachedResponse}", strValue);
            await httpContext.Response.WriteAsync(strValue);
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
            _redis.StringSet(cacheKey, response, TimeSpan.FromSeconds(10));

            ms.Position = 0;
            await ms.CopyToAsync(originalStream);
        }
        finally
        {
            httpContext.Response.Body = originalStream;
        }
    }
}