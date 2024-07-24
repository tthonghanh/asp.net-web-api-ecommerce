using Microsoft.AspNetCore.Identity;

namespace ecommerce.Models
{
    public class AppUser : IdentityUser
    {
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}