using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.ProductDtos
{
    public class UpdateProductRequestDto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Product name cannot be more than 30 characters")]
        public string Name { get; set; }
        [Required]
        [Range(1, 100000000)]
        public decimal OriginalPrice { get; set; }
        [Required]
        [Range(1, 100000000)]
        public decimal ActualPrice { get; set; }
        [Required]
        public string? Discription { get; set; }
        [Required]
        public string CategoryId { get; set; }
    }
}