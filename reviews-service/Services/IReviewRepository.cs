using ReviewService.Models;

namespace ReviewService.Services;

public interface IReviewRepository
{
    Task AddAsync(Review review);
    Task<IReadOnlyList<Review>> GetByProductAsync(string productId);
    Task<ReviewSummary> GetSummaryAsync(string productId);
}
