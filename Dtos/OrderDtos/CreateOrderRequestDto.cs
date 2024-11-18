using System.ComponentModel.DataAnnotations;

namespace ecommerce.Dtos.OrderDtos
{
    public class CreateOrderRequestDto
    {
        [Required]
        [StringLength(10, ErrorMessage = "Length of first name should be less than 10 characters")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Length of last name should be less than 10 characters")]
        public string LastName { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number should be 10 numbers")]
        [RegularExpression(@"^0+[0-9]*$")]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        [Required]
        public string District { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Payment { get; set; }
    }
}