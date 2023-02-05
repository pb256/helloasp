using System.Text;
using Microsoft.Extensions.Caching.Memory;

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
    
    public async Task Invoke(HttpContext context)
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

            await _next.Invoke(context);

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