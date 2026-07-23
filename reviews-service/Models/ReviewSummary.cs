namespace ReviewService.Models;

// Короткая сводка по продукту, сколько отзывов и средняя оценка.
public class ReviewSummary
{
    public string ProductId { get; set; } = string.Empty;
    public int Count { get; set; }
    public double AverageRating { get; set; }
}
