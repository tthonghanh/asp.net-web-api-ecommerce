using ecommerce.Dtos.CartItemDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class CartItemMapper
    {
        public static CartItemDto ToCartItemDto(this CartItem cartItem)
        {
            return new CartItemDto
            {
                Id = cartItem.Id,
                Quantity = cartItem.Quantity,
                AppUserId = cartItem.AppUserId,
                UserName = cartItem.AppUser?.UserName,
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product?.Name,
                ProductPrice = cartItem.Product?.ActualPrice,
                CategoryName = cartItem.Product?.Category.Name
            };
        }

        public static CartItem ToCartItemFromCreate(this CreateCartItemRequestDto cartItemDto, string productId, string appUserId)
        {
            return new CartItem
            {
                Quantity = cartItemDto.Quantity,
                ProductId = productId,
                AppUserId = appUserId
            };
        }
    }
}