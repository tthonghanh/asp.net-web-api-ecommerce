using ecommerce.Data;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class Product_OrderRepository : IProduct_OrderRepository
    {
        private readonly ApplicationDbContext _context;
        public Product_OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Product_Order>> GetAllProductsByOrderIdAsync(string id)
        {
            return await _context.Product_Orders
                                .Where(p => p.OrderId == id)
                                .Include(p => p.Product)
                                .ThenInclude(p => p.Category)
                                .ToListAsync();
        }
    }
}