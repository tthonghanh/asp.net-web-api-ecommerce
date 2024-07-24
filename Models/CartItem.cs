using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("CartItems")]
    public class CartItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public string AppUserId { get; set; } = string.Empty;
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public AppUser? AppUser { get; set; }
        public string ProductId { get; set; } = string.Empty;
        [DeleteBehavior(DeleteBehavior.SetNull)]
        public Product? Product { get; set; }
    }
}