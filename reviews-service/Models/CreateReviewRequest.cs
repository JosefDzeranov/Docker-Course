namespace ReviewService.Models;

// То, что присылает клиент, когда создает отзыв.
public class CreateReviewRequest
{
    public string ProductId { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Text { get; set; } = string.Empty;
}
