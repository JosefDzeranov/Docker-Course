using Microsoft.EntityFrameworkCore;
using ReviewService.Models;

namespace ReviewService.Data;

// Контекст базы данных. Одна таблица отзывов.
public class ReviewDbContext : DbContext
{
    public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base(options)
    {
    }

    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.ProductId).HasMaxLength(100);
            entity.Property(r => r.Author).HasMaxLength(100);
            entity.Property(r => r.Text).HasMaxLength(2000);

            // Индекс по продукту, чтобы быстро искать отзывы одного продукта.
            entity.HasIndex(r => r.ProductId);
        });
    }
}
