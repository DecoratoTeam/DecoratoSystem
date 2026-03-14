using DecorteeSystem;
using DecorteeSystem.MiddleWare;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Basic health (no DB)
app.MapGet("/", () => Results.Ok("DecorteeSystem API is running."));

// DB health (checks connection)
app.MapGet("/health/db", async (DecorteeDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect ? Results.Ok("DB OK") : Results.Problem("DB connection failed.", statusCode: StatusCodes.Status503ServiceUnavailable);
});

// Initialize database on startup (controlled by config)
var applyMigrations = builder.Configuration.GetValue("Database:ApplyMigrationsOnStartup", defaultValue: false);
var failFastOnMigrationError = builder.Configuration.GetValue("Database:FailFastOnMigrationError", defaultValue: true);

if (applyMigrations)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DecorteeDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Startup");

    try
    {
        var strategy = context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            logger.LogInformation("Applying EF migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("EF migrations applied successfully.");
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration failed on startup.");

        if (failFastOnMigrationError)
        {
            throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Global error handling
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<TransactionMiddleware>();

// Serve uploaded images
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
