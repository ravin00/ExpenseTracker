using AuthService.Data;
using AuthService.Dtos;
using AuthService.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AuthService.Services.AuthService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Authentication endpoints
app.MapPost("/auth/register", async (RegisterUserDto dto, AuthService.Services.AuthService authService) =>
{
    try
    {
        var user = await authService.RegisterAsync(dto);
        return Results.Created($"/users/{user.Id}", new { user.Id, user.Username, user.Email });
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new { error = ex.Message });
    }
})
.WithName("Register")
.WithOpenApi();

app.MapPost("/auth/login", async (LoginUserDto dto, AuthService.Services.AuthService authService) =>
{
    var user = await authService.AuthenticateAsync(dto);
    if (user == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { user.Id, user.Username, user.Email });
})
.WithName("Login")
.WithOpenApi();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
