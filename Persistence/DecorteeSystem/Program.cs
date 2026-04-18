using DecorteeSystem;
using DecorteeSystem.MiddleWare;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Apply migrations and seed database on startup (runs in all environments)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DecorteeDbContext>();

    // Run migrations (safe to run multiple times)
    await context.Database.MigrateAsync();

    // Seed only if empty
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // Enable Swagger in production too (for mobile app testing)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Serve static files (wwwroot/static/ for images, wwwroot/uploads/ for profile pictures)
app.UseStaticFiles();

app.UseMiddleware<TransactionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
