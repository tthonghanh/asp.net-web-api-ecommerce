using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Dtos.Account
{
    [Index(nameof(Email), IsUnique = true)]
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(10, ErrorMessage = "Phonenumber should have 10 numbers")]
        [RegularExpression(@"^0+[0-9]*$")]
        public string PhoneNumber { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}