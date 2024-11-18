using ecommerce.Dtos.OrderDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class OrderMapper
    {
        public static OrderDto ToOrderDto(this Order orderModel)
        {
            return new OrderDto
            {
                Id = orderModel.Id,
                FirstName = orderModel.FirstName,
                LastName = orderModel.LastName,
                PhoneNumber = orderModel.PhoneNumber,
                Address1 = orderModel.Address1,
                Address2 = orderModel.Address2,
                District = orderModel.District,
                City = orderModel.City,
                Payment = orderModel.Payment,
                CreateAt = orderModel.CreateAt,
                UpdateAt = orderModel.UpdateAt,
                SubToTal = orderModel.SubToTal,
                ShippingPrice = orderModel.ShippingPrice,
                Discount = orderModel.Discount,
                TotalInvoicement = orderModel.TotalInvoicement,
                Status = orderModel.Status,
                IsCanceled = orderModel.IsCanceled,
                // AppUserId = orderModel.AppUserId
            };
        }

        public static Order ToOrderFromCreate(this CreateOrderRequestDto orderDto, string userId) {
            return new Order {
                FirstName = orderDto.FirstName,
                LastName = orderDto.LastName,
                PhoneNumber = orderDto.PhoneNumber,
                Address1 = orderDto.Address1,
                Address2 = orderDto.Address2,
                District = orderDto.District,
                City = orderDto.City,
                Payment = orderDto.Payment,
                AppUserId = userId
            };
        }

        public static OrderDetailDto ToOrderDetailDto(this Order orderModel) {
            return new OrderDetailDto {
                OrderDto = orderModel.ToOrderDto(),
                ProductList = orderModel.Product_Orders
                                        .Select(p => p.ToProductOrderDto())
                                        .ToList()
            };
        }
    }
}