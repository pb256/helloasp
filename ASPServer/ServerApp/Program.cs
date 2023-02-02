using Microsoft.EntityFrameworkCore;
using ServerApp.Middlewares;
using ServerApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddMemoryCache()
    .AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"))
    // .AddSingleton<RequestParser>() // IMiddleware 상속받으면 미리 서비스 등록 해줘야함
    // .AddSingleton<ResponseCache>()
    .AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseDeveloperExceptionPage();
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseMiddleware<RequestParser>();
// app.UseMiddleware<ResponseCache>();

app.MapControllers();
app.Run();