namespace ecommerce.Dtos.FeedbackDtos
{
    public class FeedbackDto
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public int Stars { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string? CreateBy { get; set; }
    }
}