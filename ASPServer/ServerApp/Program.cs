using ServerApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMemoryCache()
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseMiddleware<RequestParser>();
app.UseMiddleware<ResponseCache>();

app.MapControllers();
app.Run();