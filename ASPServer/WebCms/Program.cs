using Api;
using Microsoft.EntityFrameworkCore;
using Models.Contexts;
using Microsoft.Extensions.DependencyInjection;
using WebCms.Data;
using WebCms.SeedData;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WebCmsContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebCmsContext") ?? throw new InvalidOperationException("Connection string 'WebCmsContext' not found.")));
IConfigurationRoot configurationRoot = builder.Configuration;

// database connection string
var connectionString = configurationRoot
    .GetConnectionString("MySql");

// Add services to the container.
var services = builder.Services;
services
    .AddApiService()
    .AddDbContext<GameContext>(options => options.UseMySQL(connectionString));

services.AddRazorPages();

var app = builder.Build();

// 초기 시드 데이터 추가
using (var scope = app.Services.CreateScope())
{
    SeedData.Initialize(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();