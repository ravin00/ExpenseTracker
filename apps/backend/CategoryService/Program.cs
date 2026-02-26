using CategoryService.Data;
using CategoryService.Middleware;
using CategoryService.Repositories;
using CategoryService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using Prometheus;

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/categoryservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // DbContext with validation
    var connectionString = builder.Configuration.GetConnectionString("CategoryDb");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("CategoryDb connection string is not configured");
    }

    builder.Services.AddDbContext<CategoryDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Services & repositories
    builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
    builder.Services.AddScoped<ICategoryService, CategoryService.Services.CategoryService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Enhanced Swagger configuration
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ExpenseTracker Category API",
            Version = "v1",
            Description = "Category management service for ExpenseTracker application"
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    // JWT authentication with validation
    var jwtSecret = builder.Configuration["Jwt:Secret"];
    if (string.IsNullOrEmpty(jwtSecret))
    {
        jwtSecret = "default-secret-key-for-development";
        Log.Warning("Using default JWT secret. This should not be used in production!");
    }

    var key = Encoding.ASCII.GetBytes(jwtSecret);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set to true in production
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

    // Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwaggerUI", policy =>
    {
        policy
            .WithOrigins("http://localhost:8088")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

    // Add health checks
    builder.Services.AddHealthChecks()
        .AddCheck("self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

    var app = builder.Build();

    // Configure middleware pipeline
    app.UseMiddleware<GlobalExceptionMiddleware>();
    app.UseCors("AllowSwaggerUI");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Category API V1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHttpMetrics();

    app.MapControllers();
    app.MapHealthChecks("/health");
    app.MapMetrics();

    // Auto-migrate database
    try
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CategoryDbContext>();
        context.Database.EnsureCreated();
        Log.Information("Category database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to initialize category database");
        Log.Warning("Service will start but database features won't work until SQL Server is available");
    }

    Log.Information("Category Service starting...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Category Service failed to start");
}
finally
{
    Log.CloseAndFlush();
}