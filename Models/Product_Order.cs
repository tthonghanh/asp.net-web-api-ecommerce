using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("Product_Orders")]
    public class Product_Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? OrderId { get; set; }
        public string? ProductId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalPrice { get; set; }

        // Navigation
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Order Order { get; set; } = null!;
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Product Product { get; set; } = null!;
    }
}