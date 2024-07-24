using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product_Order> Product_Orders { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole> {
                new IdentityRole {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole {
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}