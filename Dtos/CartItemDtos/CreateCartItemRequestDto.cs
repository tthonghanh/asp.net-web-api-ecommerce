using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.CartItemDtos
{
    public class CreateCartItemRequestDto
    {
        [Required]
        [Range(1,10, ErrorMessage = "At least 1 item and at most 10 items")]
        public int Quantity { get; set; }
    }
}