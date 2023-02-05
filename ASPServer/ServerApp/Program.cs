using Api;
using ServerApp.Middlewares;
using ServerApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Singleton으로 IMemoryCache를 래핑한 서비스를 추가했는데.. 뭔가 잘 안되서 복구함
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