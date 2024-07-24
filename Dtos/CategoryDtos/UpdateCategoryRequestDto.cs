using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.CategoryDtos
{
    public class UpdateCategoryRequestDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Category name cannot be over 20 character")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$", ErrorMessage = "Category name should only contain alphabet character and the first letter should be captital letter")]
        public string Name { get; set; } = null!;

        [Required]
        public string ParentCategory { get; set; } = null!;
    }
}