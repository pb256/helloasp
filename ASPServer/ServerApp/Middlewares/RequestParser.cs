using Newtonsoft.Json.Linq;

namespace ServerApp.Middlewares;

public class RequestParser : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        using (var sr = new StreamReader(context.Request.Body))
        {
            var text = await sr.ReadToEndAsync();
            var json = JObject.Parse(text);

            // Items: 임시 dictionary같은것
            context.Items["text"] = text;
            context.Items["json"] = json;
        }
        await next.Invoke(context);
    }
}