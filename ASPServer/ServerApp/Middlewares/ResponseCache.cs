using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace ServerApp.Middlewares;

public class ResponseCache : IMiddleware
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ResponseCache> _logger;
    
    public ResponseCache(
        IMemoryCache memoryCache,
        ILogger<ResponseCache> logger)
    {
        _memoryCache = memoryCache;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Items["text"] as string;
        
        if (_memoryCache.TryGetValue(request, out string cachedResponse))
        {
            _logger.LogInformation($"[{nameof(ResponseCache)}] FromCache: {cachedResponse}");
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        var originalStream = context.Response.Body;

        try
        {
            await using var ms = new MemoryStream();
            context.Response.Body = ms;

            await next.Invoke(context);

            var response = Encoding.UTF8.GetString(ms.ToArray());
            _logger.LogInformation($"[{nameof(ResponseCache)}] FromController: {response}");
            _memoryCache.Set(request, response, TimeSpan.FromSeconds(5));

            ms.Position = 0;
            await ms.CopyToAsync(originalStream);
        }
        finally
        {
            context.Response.Body = originalStream;
        }
    }
}