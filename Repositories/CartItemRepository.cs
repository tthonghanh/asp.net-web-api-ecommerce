using ecommerce.Data;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly ApplicationDbContext _context;
        public CartItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem?> CreateCartItemAsync(CartItem cartItemModel)
        {
            var userCart = await GetCartItemsByCustomerIdAsync(cartItemModel.AppUserId);
            if (userCart != null) {
                var product = userCart.FirstOrDefault(c => c.ProductId == cartItemModel.ProductId);
                if (product != null) {
                    if (product.Quantity + cartItemModel.Quantity > 10) return null;
                    else {
                        product.Quantity += cartItemModel.Quantity;
                        await _context.SaveChangesAsync();
                        return product;
                    }
                }
            }
            await _context.CartItems.AddAsync(cartItemModel);
            await _context.SaveChangesAsync();

            return cartItemModel;
        }

        public async Task<bool> DeleteAllCartItemsAsync(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CartItem?> DeleteCartItemAsync(string id, string appUserId)
        {
            var cartItemModel = await _context.CartItems.FirstOrDefaultAsync(cartItem => cartItem.Id == id);

            if (cartItemModel == null) return null;
            if (cartItemModel.AppUserId != appUserId) return null;

            _context.CartItems.Remove(cartItemModel);
            await _context.SaveChangesAsync();

            return cartItemModel;
        }

        public async Task<List<CartItem>> GetAllCartItemsAsync()
        {
            return await _context.CartItems
                            .Include(cart => cart.Product)
                            .ThenInclude(product => product!.Category)
                            .ToListAsync();
        }

        public async Task<List<CartItem>> GetCartItemsByCustomerIdAsync(string id)
        {
            return await _context.CartItems
                            .Include(cart => cart.Product)
                            .ThenInclude(product => product!.Category)
                            .Where(c => c.AppUserId == id)
                            .ToListAsync();
        }

        public async Task<CartItem?> UpdateCartItemAsync(string id, int quantity, string appUserId)
        {
            var existingCartItem = await _context.CartItems.FirstOrDefaultAsync(cart => cart.Id == id);

            if (existingCartItem == null) return null;
            if (existingCartItem.AppUserId != appUserId) return null;

            existingCartItem.Quantity = quantity;
            await _context.SaveChangesAsync();

            return existingCartItem;
        }
    }
}