using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPITest.Models.DB;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("tmdb", c =>
{
    c.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<filmplattformContext>(options => options.UseMySQL("Server=127.0.0.1; Port=3306; Database=filmplattform; Uid=root; Pwd=secret;"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo(){Title = "Filmplattform", Version = "v1"});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Filmplattform");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();