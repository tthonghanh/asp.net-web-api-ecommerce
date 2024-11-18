using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    [Table("Categories")]
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string? ParentCategory { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}