using System.ComponentModel.DataAnnotations.Schema;

namespace ecommerce.Models
{
    [Table("Categories")]
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string ParentCategory { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new List<Product>();
    }
}