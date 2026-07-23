using Microsoft.EntityFrameworkCore;
using ReviewService.Data;
using ReviewService.Models;

namespace ReviewService.Services;

public class EfReviewRepository : IReviewRepository
{
    private readonly ReviewDbContext _db;

    public EfReviewRepository(ReviewDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Review review)
    {
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<Review>> GetByProductAsync(string productId)
    {
        return await _db.Reviews
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<ReviewSummary> GetSummaryAsync(string productId)
    {
        var query = _db.Reviews.Where(r => r.ProductId == productId);

        var count = await query.CountAsync();
        var average = count == 0
            ? 0
            : await query.AverageAsync(r => (double)r.Rating);

        return new ReviewSummary
        {
            ProductId = productId,
            Count = count,
            AverageRating = Math.Round(average, 2)
        };
    }
}
