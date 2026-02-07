using AnalyticsService.Data;
using AnalyticsService.Middleware;
using AnalyticsService.Repositories;
using AnalyticsService.Services;
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
    .WriteTo.File("logs/analyticsservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // DbContext with validation
    var connectionString = builder.Configuration.GetConnectionString("AnalyticsDb");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("AnalyticsDb connection string is not configured");
    }

    builder.Services.AddDbContext<AnalyticsDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Services & repositories
    builder.Services.AddScoped<IAnalyticsRepository, AnalyticsRepository>();
    builder.Services.AddScoped<IAnalyticsService, AnalyticsService.Services.AnalyticsService>();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });
    builder.Services.AddEndpointsApiExplorer();

    // Enhanced Swagger configuration
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ExpenseTracker Analytics API",
            Version = "v1",
            Description = "Analytics and reporting service for ExpenseTracker application"
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
            .WithOrigins("http://localhost:8088", "http://localhost:5173")
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
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Analytics API V1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseAuthentication();
    app.UseAuthorization();
    // Collect default HTTP metrics (requests, durations, in-progress)
    app.UseHttpMetrics();

    app.MapControllers();
    app.MapHealthChecks("/health");
    // Expose Prometheus metrics
    app.MapMetrics();

    // Auto-migrate database
    try
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AnalyticsDbContext>();
        context.Database.Migrate();
        Log.Information("Analytics database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to initialize analytics database");
        Log.Warning("Service will start but database features won't work until SQL Server is available");
    }

    Log.Information("Analytics Service starting...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Analytics Service failed to start");
}
finally
{
    Log.CloseAndFlush();
}