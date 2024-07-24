using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("ProductImages")]
    public class ProductImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImageName { get; set; } = string.Empty;
        public string ExtensionType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = [];
        public string ProductId { get; set; } = string.Empty;
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Product Product { get; set; } = null!;
    }
}