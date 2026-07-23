using Microsoft.AspNetCore.Mvc;
using ReviewService.Models;
using ReviewService.Services;

namespace ReviewService.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewRepository _repository;

    public ReviewsController(IReviewRepository repository)
    {
        _repository = repository;
    }

    // POST /reviews  добавить новый отзыв.
    [HttpPost]
    public async Task<ActionResult<Review>> Create(CreateReviewRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.ProductId))
            return BadRequest("Не указан продукт");

        if (request.Rating < 1 || request.Rating > 5)
            return BadRequest("Оценка должна быть от 1 до 5");

        var review = new Review
        {
            Id = Guid.CreateVersion7(),
            ProductId = request.ProductId,
            Author = string.IsNullOrWhiteSpace(request.Author) ? "Аноним" : request.Author,
            Rating = request.Rating,
            Text = request.Text,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _repository.AddAsync(review);
        return Ok(review);
    }

    // GET /reviews/{productId}  все отзывы продукта.
    [HttpGet("{productId}")]
    public async Task<ActionResult<IReadOnlyList<Review>>> GetByProduct(string productId)
    {
        var reviews = await _repository.GetByProductAsync(productId);
        return Ok(reviews);
    }

    // GET /reviews/{productId}/summary  сводка по продукту.
    [HttpGet("{productId}/summary")]
    public async Task<ActionResult<ReviewSummary>> GetSummary(string productId)
    {
        var summary = await _repository.GetSummaryAsync(productId);
        return Ok(summary);
    }
}
