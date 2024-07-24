using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IProduct_OrderRepository
    {
        Task<List<Product_Order>> GetAllProductsByOrderIdAsync(string id);
    }
}