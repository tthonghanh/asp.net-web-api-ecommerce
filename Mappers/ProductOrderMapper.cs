using ecommerce.Dtos.ProductOrderDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class ProductOrderMapper
    {
        public static ProductOrderDto ToProductOrderDto(this Product_Order productOrderModel)
        {
            return new ProductOrderDto
            {
                Id = productOrderModel.Id,
                OrderId = productOrderModel.OrderId,
                ProductId = productOrderModel.ProductId,
                ProductName = productOrderModel.Product.Name,
                CategoryName = productOrderModel.Product.Category.Name,
                Price = productOrderModel.Price,
                Quantity = productOrderModel.Quantity,
                TotalPrice = productOrderModel.TotalPrice
            };
        }
        public static Product_Order ToProductOrderFromCartItem(this CartItem cartItem, string orderId)
        {
            return new Product_Order
            {
                OrderId = orderId,
                ProductId = cartItem.ProductId,
                Price = cartItem.Product!.ActualPrice,
                Quantity = cartItem.Quantity,
                TotalPrice = cartItem.Product.ActualPrice * cartItem.Quantity
            };
        }
    }
}