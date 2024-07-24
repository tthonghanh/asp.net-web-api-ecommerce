using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("Orders")]
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal SubToTal { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal ShippingPrice { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Discount { get; set; } = 0;
        [Column(TypeName = "decimal(18, 2)")]
        [DataType(DataType.Currency)]
        public decimal TotalInvoicement { get; set; } = 0;
        public string Status { get; set; } = "In progress";
        public bool IsCanceled { get; set; } = false;
        public string? AppUserId { get; set; }
        [DeleteBehavior(DeleteBehavior.SetNull)]
        public AppUser? AppUser { get; set; }
        public List<Product_Order> Product_Orders { get; set; } = new List<Product_Order>();
    }
}