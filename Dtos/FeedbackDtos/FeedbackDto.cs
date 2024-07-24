namespace ecommerce.Dtos.FeedbackDtos
{
    public class FeedbackDto
    {
        public string Id { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public int Stars { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
        public string? ProductId { get; set; }
    }
}