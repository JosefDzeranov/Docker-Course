namespace ReviewService.Models;

// Один отзыв о продукте.
public class Review
{
    public Guid Id { get; set; }

    // К какому продукту относится отзыв, например course-1.
    public string ProductId { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    // Оценка от 1 до 5.
    public int Rating { get; set; }

    public string Text { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }
}
