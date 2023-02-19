using Api;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using ServerApp.Middlewares;
using ServerApp.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
IConfigurationRoot configurationRoot = builder.Configuration;

var dbConnectionString = configurationRoot
    .GetConnectionString("MySql");

var redisConnectionString = configurationRoot
    .GetConnectionString("Redis");

var redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);

builder.Services
    .AddScoped<RequestContext>()
    .AddSingleton(redisConnection.GetDatabase())
    .AddApiService()
    .AddDbContext<GameContext>(options => options.UseMySQL(dbConnectionString))
    .AddControllers();

builder.Host
    .ConfigureLogging(loggingBuilder => loggingBuilder.AddLog4Net());

var app = builder.Build();

app.UseMiddleware<RequestParser>();
app.UseMiddleware<ResponseCache>();

app.MapControllers();
app.Run();