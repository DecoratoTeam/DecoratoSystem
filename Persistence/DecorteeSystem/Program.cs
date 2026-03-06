using DecorteeSystem;
using DecorteeSystem.MiddleWare;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// Apply migrations and seed database in Development environment
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DecorteeDbContext>();
    
    // Apply pending migrations automatically
    await context.Database.MigrateAsync();
    
    // Seed the database
    await DbSeeder.SeedAsync(context);
}

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TransactionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
