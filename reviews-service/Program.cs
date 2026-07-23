using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using ReviewService.Data;
using ReviewService.Services;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("Postgres");

builder.Services.AddDbContext<ReviewDbContext>(options => options.UseNpgsql(connection));

builder.Services.AddScoped<IReviewRepository, EfReviewRepository>();

builder.Services.AddControllers();

// Генерация OpenAPI-спеки, на ней строится визуальный интерфейс.
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ReviewDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.MapControllers();

// OpenAPI-спека на /openapi/v1.json и визуальный интерфейс Scalar на /scalar/v1.
app.MapOpenApi();
app.MapScalarApiReference();

// Простой healthcheck, удобно проверить что сервис жив.
app.MapGet("/health", () => Results.Ok("healthy"));

app.Run();
