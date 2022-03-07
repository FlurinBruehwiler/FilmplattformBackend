using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebAPITest.Factories;
using WebAPITest.Models.DB;
using WebAPITest.Services;
using static System.Text.Encoding;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddHttpClient("tmdb", c =>
{
    c.BaseAddress = new Uri(builder.Configuration.GetValue<string>("TmdbAPI"));
});
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<FilmplattformContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetValue<string>("ConnectionString"));
    options.EnableSensitiveDataLogging();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

//Add Services
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<PersonTypeService>();
builder.Services.AddScoped<TmdbService>();

//Add Factories
builder.Services.AddScoped<WatcheventFactory>();
builder.Services.AddScoped<DtoMovieFactory>();
builder.Services.AddScoped<GenreFactory>();
builder.Services.AddScoped<MovieFactory>();
builder.Services.AddScoped<PersonFactory>();
builder.Services.AddScoped<PersonTypeFactory>();


builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c =>
{
    //c.SwaggerDoc("v1", new OpenApiInfo{Title = "Filmplattform", Version = "v1"});
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token} \")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:Secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();