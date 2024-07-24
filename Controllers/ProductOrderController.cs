using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductOrderController : ControllerBase
    {
        private readonly IProduct_OrderRepository _productOrderRepository;
        public ProductOrderController(IProduct_OrderRepository productOrderRepo)
        {
            _productOrderRepository = productOrderRepo;
        }

        [HttpGet("GetAllProductsByOrderId/{orderId}")]
        public async Task<IActionResult> GetAllProductsByOrderId(string orderId) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var productList = await _productOrderRepository.GetAllProductsByOrderIdAsync(orderId);

            return Ok(productList.Select(p => p.ToProductOrderDto()));
        }
    }
}