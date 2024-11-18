using System.Data;
using ClosedXML.Excel;
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
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQuery orderQuery)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var orderModel = await _orderRepo.GetAllOrdersAsync(orderQuery);

                return Ok(orderModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

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

        [HttpGet("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var _empData = _orderRepo.GetEmpData();
            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet1 = wb.AddWorksheet(_empData, "Order List");
                // sheet234 are the same
                sheet1.Column(1).Style.Font.FontColor = XLColor.Red; // set color to the text
                sheet1.Columns(2, 4).Style.Font.FontColor = XLColor.AliceBlue; // set color from col2 to col4

                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Gray; // set color to row, only cells have data
                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                sheet1.Row(1).Style.Font.Italic = true;

                sheet1.Row(2).Cells(1, 3).Style.Fill.BackgroundColor = XLColor.Gray; // set color to 1 row, only cells 1 - 3
                sheet1.Rows(3, 5).Style.Fill.BackgroundColor = XLColor.Almond;

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.speadsheetml.sheet", "Orders.xlsx");
                };
            }
        }

        [HttpGet("GetBill")]
        public async Task<IActionResult> GetBill(string orderId)
        {
            var filePath = await _orderRepo.MergeReport(orderId);

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", Path.GetFileName(filePath));
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

            return CreatedAtAction(nameof(GetOrderById), new { orderId = orderModel.Id }, orderModel.ToOrderDetailDto());
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