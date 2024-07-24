using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.Dtos.Account
{
    public class UpdateAppUserRequestDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        [StringLength(10, ErrorMessage = "Phonenumber should have 10 numbers")]
        [RegularExpression(@"^0+[0-9]*$")]
        public string PhoneNumber { get; set; } = null!;
    }
}