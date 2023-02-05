using Newtonsoft.Json;
using ServerApp.Services;

namespace ServerApp.Middlewares;

public class RequestParser
{
    private readonly RequestDelegate _next;
    
    public RequestParser(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, RequestContext requestContext)
    {
        var sr = new StreamReader(httpContext.Request.Body);
        var text = await sr.ReadToEndAsync();

        try
        {
            JsonConvert.PopulateObject(text, requestContext);
        }
        catch
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        if (string.IsNullOrEmpty(requestContext.Uid)
            || requestContext.Actions == null)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }
        
        await _next.Invoke(httpContext);
    }
}