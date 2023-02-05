using Newtonsoft.Json.Linq;

namespace ServerApp.Middlewares;

public class RequestParser
{
    private readonly RequestDelegate _next;
    public RequestParser(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        using (var sr = new StreamReader(context.Request.Body))
        {
            var text = await sr.ReadToEndAsync();
            var json = JObject.Parse(text);

            // Items: 임시 dictionary같은것
            context.Items["text"] = text;
            context.Items["json"] = json;
        }
        await _next.Invoke(context);
    }
}