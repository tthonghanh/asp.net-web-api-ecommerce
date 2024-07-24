using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.FeedbackDtos
{
    public class UpdateFeedbackRequestDto
    {
        [MaxLength(200, ErrorMessage = "Comment should be less than 200 characters")]
        public string Content { get; set; } = string.Empty;
        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }
    }
}