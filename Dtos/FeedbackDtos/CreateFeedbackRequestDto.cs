using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.FeedbackDtos
{
    public class CreateFeedbackRequestDto
    {
        [MaxLength(200, ErrorMessage = "Comment should be less than 200 characters")]
        public string Content { get; set; }
        [Required]
        [Range(1, 5)]
        public int Stars { get; set; }
    }
}