using BudgetService.Data;
using BudgetService.Middleware;
using BudgetService.Repositories;
using BudgetService.Services;
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
    .WriteTo.File("logs/budgetservice-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // DbContext with validation
    var connectionString = builder.Configuration.GetConnectionString("BudgetDb");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("BudgetDb connection string is not configured");
    }

    builder.Services.AddDbContext<BudgetDbContext>(options =>
        options.UseNpgsql(connectionString));

    // Services & repositories
    builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
    builder.Services.AddScoped<IBudgetService, BudgetService.Services.BudgetService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    // Enhanced Swagger configuration
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "ExpenseTracker Budget API",
            Version = "v1",
            Description = "Budget management service for ExpenseTracker application"
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
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
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

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Budget API V1");
            c.RoutePrefix = "swagger";
        });
    }

    app.UseCors("AllowAll");
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
        var context = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
        context.Database.EnsureCreated();
        Log.Information("Budget database initialized successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Failed to initialize budget database");
        Log.Warning("Service will start but database features won't work until SQL Server is available");
    }

    Log.Information("Budget Service starting...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Budget Service failed to start");
}
finally
{
    Log.CloseAndFlush();
}