using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.ProductImageDtos
{
    public class UpdateProductImageRequestDto
    {
        public IFormFile ImageDetail { get; set; } = null!;
        [Required]
        [MaxLength(30, ErrorMessage = "ImageName cannot be more than 20 characters")]
        public string? ImageName { get; set; }
    }
}