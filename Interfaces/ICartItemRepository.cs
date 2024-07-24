using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface ICartItemRepository
    {
        Task<List<CartItem>> GetAllCartItemsAsync();
        Task<List<CartItem>> GetCartItemsByCustomerIdAsync(string id);
        Task<CartItem?> CreateCartItemAsync(CartItem cartItemModel);
        Task<CartItem?> UpdateCartItemAsync(string id, int quantity, string appUserId);
        Task<CartItem?> DeleteCartItemAsync(string id, string appUserId);
        Task<bool> DeleteAllCartItemsAsync(List<CartItem> cartItems);
    }
}