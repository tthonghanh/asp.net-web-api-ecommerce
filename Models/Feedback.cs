using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Models
{
    [Table("Feedbacks")]
    public class Feedback
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; }
        public int Stars { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string? ProductId { get; set; }
        public string? AppUserId { get; set; }

        // Navigation
        [DeleteBehavior(DeleteBehavior.Cascade)]
        public Product? Product { get; set; }
        [DeleteBehavior(DeleteBehavior.SetNull)]
        public AppUser? AppUser { get; set; }
    }
}