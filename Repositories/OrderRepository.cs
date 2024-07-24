using ecommerce.Data;
using ecommerce.Dtos.OrderDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> CancelOrderAsync(string orderId, string appUserId)
        {
            var orderModel = await GetOrderByIdAsync(orderId);

            if (orderModel == null) return null;
            if (orderModel.AppUserId != appUserId) return null;

            orderModel.IsCanceled = true;
            orderModel.Status = "Canceled";

            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<Order> CreateOrderAsync(Order orderModel, List<Product_Order> product_Orders)
        {
            await _context.Product_Orders.AddRangeAsync(product_Orders);
            await _context.Orders.AddAsync(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<Order?> DeleteOrderAsync(string orderId)
        {
            var orderModel = await GetOrderByIdAsync(orderId);
            if (orderModel == null) return null;

            _context.Remove(orderModel);
            await _context.SaveChangesAsync();

            return orderModel;
        }

        public async Task<List<Order>> GetAllOrdersAsync(OrderQuery orderQuery)
        {
            var orders = _context.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(orderQuery.DateStart.ToString()) && !string.IsNullOrWhiteSpace(orderQuery.DateEnd.ToString())) {
                orders = orders.Where(order => DateOnly.FromDateTime(order.CreateAt) >= orderQuery.DateStart && DateOnly.FromDateTime(order.CreateAt) <= orderQuery.DateEnd);
            }
            else if (!string.IsNullOrWhiteSpace(orderQuery.DateStart.ToString())) {
                orders = orders.Where(order => DateOnly.FromDateTime(order.CreateAt) >= orderQuery.DateStart);
            }
            else if (!string.IsNullOrWhiteSpace(orderQuery.DateEnd.ToString())) {
                orders = orders.Where(order => DateOnly.FromDateTime(order.CreateAt) <= orderQuery.DateEnd);
            }

            return await orders.ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            return await _context.Orders
                                .Include(order => order.Product_Orders)
                                .ThenInclude(p => p.Product)
                                .ThenInclude(p => p.Category)
                                .FirstOrDefaultAsync(order => order.Id == id);
        }

        public async Task<List<Order>> GetOrdersByCustomerIdAsync(string id)
        {
            return await _context.Orders
                            .Where(order => order.AppUserId == id)
                            .ToListAsync();
        }

        public async Task<Order?> UpdateOrderAsync(string id, UpdateOrderRequestDto orderDto, string appUserId)
        {
            var existingOrder = await _context.Orders.FirstOrDefaultAsync(order => order.Id == id);

            if (existingOrder == null) return null;
            if (existingOrder.AppUserId != appUserId) return null;

            existingOrder.FirstName = orderDto.FirstName;
            existingOrder.LastName = orderDto.LastName;
            existingOrder.PhoneNumber = orderDto.PhoneNumber;
            existingOrder.Address1 = orderDto.Address1;
            existingOrder.Address2 = orderDto.Address2;
            existingOrder.District = orderDto.District;
            existingOrder.City = orderDto.City;
            existingOrder.Payment = orderDto.Payment;
            existingOrder.UpdateAt = DateTime.Now;

            await _context.SaveChangesAsync();
            return existingOrder;
        }
    }
}