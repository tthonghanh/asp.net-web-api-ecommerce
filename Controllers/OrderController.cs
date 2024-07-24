using ecommerce.Dtos.OrderDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IUserService _userService;
        private readonly ICartItemRepository _cartItemRepo;
        public OrderController(IOrderRepository orderRepo, IUserService userService, ICartItemRepository cartItemRepo)
        {
            _orderRepo = orderRepo;
            _userService = userService;
            _cartItemRepo = cartItemRepo;
        }

        [HttpGet("GetAllOrders")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQuery orderQuery)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var orderModel = await _orderRepo.GetAllOrdersAsync(orderQuery);

            return Ok(orderModel.Select(order => order.ToOrderDto()));
        }

        [HttpGet("GetOrdersByCustomerId/{appUserId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersByCustomerId([FromRoute] string appUserId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            if (User.IsInRole("Customer"))
            {
                if (appUserId != appUser.Id)
                {
                    return RedirectToAction(nameof(GetOrdersByCustomerId), new { appUserId = appUser.Id });
                }
            }

            var orderModel = await _orderRepo.GetOrdersByCustomerIdAsync(appUserId);
            return Ok(orderModel.Select(order => order.ToOrderDto()));
        }

        [HttpGet("GetOrderById/{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrderById([FromRoute] string orderId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var orderModel = await _orderRepo.GetOrderByIdAsync(orderId);

            if ((orderModel == null) ||
                (User.IsInRole("Customer") && orderModel.AppUserId != appUser.Id)) return NotFound("Order not found");

            return Ok(orderModel.ToOrderDetailDto());
        }

        [HttpPost("PlaceOrder")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromForm] CreateOrderRequestDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var cartItems = await _cartItemRepo.GetCartItemsByCustomerIdAsync(appUser.Id);
            if (cartItems.Count == 0) return BadRequest("Cart does not have any product");

            var orderModel = orderDto.ToOrderFromCreate(appUser.Id);
            var productOrderModel = cartItems.Select(cartItem => cartItem.ToProductOrderFromCartItem(orderModel.Id)).ToList();

            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    orderModel.SubToTal += item.Quantity * item.Product.OriginalPrice;
                    orderModel.TotalInvoicement += item.Quantity * item.Product.ActualPrice;
                }
            }
            orderModel.Discount = (orderModel.SubToTal - orderModel.TotalInvoicement) / orderModel.SubToTal;
            orderModel.TotalInvoicement += orderModel.ShippingPrice;

            await _orderRepo.CreateOrderAsync(orderModel, productOrderModel);
            await _cartItemRepo.DeleteAllCartItemsAsync(cartItems);

            return CreatedAtAction(nameof(GetOrderById), new {orderId = orderModel.Id}, orderModel.ToOrderDetailDto());
        }

        [HttpPut("EditOrderInfo/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateOrder([FromRoute] string orderId, [FromForm] UpdateOrderRequestDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var orderModel = await _orderRepo.UpdateOrderAsync(orderId, orderDto, appUser.Id);
            if (orderModel == null) return NotFound("Order not found");

            return Ok(orderModel.ToOrderDto());
        }

        [HttpPut("CancelOrder/{orderId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var orderModel = await _orderRepo.CancelOrderAsync(orderId, appUser.Id);
            if (orderModel == null) return NotFound("Order not found");

            return Ok(orderModel.ToOrderDto());
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteOrder([FromRoute] string orderId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var orderModel = await _orderRepo.DeleteOrderAsync(orderId);
            if (orderModel == null) return NotFound("Order not found");

            return Ok("Delete successfully");
        }
    }
}