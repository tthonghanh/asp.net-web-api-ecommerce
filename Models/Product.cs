using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("Products")]
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OriginalPrice { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ActualPrice { get; set; }
        public string? Discription { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string? CategoryId { get; set; }
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Category Category { get; set; } = null!;
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public List<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public List<Product_Order> Product_Orders { get; set; } = new List<Product_Order>();
    }
}