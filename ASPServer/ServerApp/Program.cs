using Api;
using ServerApp.Middlewares;
using ServerApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddScoped<RequestContext>()
    .AddMemoryCache()
    .AddApiService()
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.UseMiddleware<RequestParser>();
app.UseMiddleware<ResponseCache>();

app.MapControllers();
app.Run();