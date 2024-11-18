using System.Data;
using ecommerce.Dtos.OrderDtos;
using ecommerce.Helpers;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllOrdersAsync(OrderQuery orderQuery);
        Task<List<Order>> GetOrdersByCustomerIdAsync(string id);
        Task<Order?> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(Order orderModel, List<Product_Order> product_Orders);
        Task<Order?> UpdateOrderAsync(string orderId, UpdateOrderRequestDto orderDto, string appUserId);
        Task<Order?> CancelOrderAsync(string orderId, string appUserId);
        Task<Order?> DeleteOrderAsync(string orderId);
        DataTable GetEmpData();
        Task<string?> MergeReport(string orderId);
    }
}