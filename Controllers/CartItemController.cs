using ecommerce.Dtos.CartItemDtos;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserService _userService;
        public CartItemController(ICartItemRepository cartItemRepo, IProductRepository productRepo, IUserService userService)
        {
            _cartItemRepo = cartItemRepo;
            _productRepo = productRepo;
            _userService = userService;
        }

        [HttpGet("GetAllCartItems")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCartItems()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cartItemModel = await _cartItemRepo.GetAllCartItemsAsync();

            return Ok(cartItemModel.Select(cart => cart.ToCartItemDto()));
        }

        [HttpGet("GetCartItemsByCustomerId/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCartItemsByCustomerId([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var cartItemModel = await _cartItemRepo.GetCartItemsByCustomerIdAsync(id);

            return Ok(cartItemModel.Select(cart => cart.ToCartItemDto()));
        }

        [HttpGet("GetCustomerCart")]
        [Authorize]
        public async Task<IActionResult> GetCustomerCart()
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var cartItems = await _cartItemRepo.GetCartItemsByCustomerIdAsync(appUser.Id);

            return Ok(cartItems.Select(cartItem => cartItem.ToCartItemDto()));
        }

        [HttpPost("AddProductToCart/{productId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateCartItem([FromRoute] string productId, [FromForm] CreateCartItemRequestDto cartItemDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var product = await _productRepo.ProductExist(productId);
            if (!product) return NotFound("Product not found");

            var cartItemModel = cartItemDto.ToCartItemFromCreate(productId, appUser.Id);

            var createdCart = await _cartItemRepo.CreateCartItemAsync(cartItemModel);
            if (createdCart == null) return BadRequest("Number of products in cart should not exceed 10 items");

            return Ok(createdCart.ToCartItemDto());
        }

        [HttpPut("UpdateCartItemById/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateCartItemById([FromRoute] string id, [FromForm] int quantity)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var cartItemModel = await _cartItemRepo.UpdateCartItemAsync(id, quantity, appUser.Id);
            if (cartItemModel == null) return NotFound("Product does not exist in cart");

            return Ok(cartItemModel.ToCartItemDto());
        }

        [HttpDelete("DeleteCartItemById/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItemById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var cartItemModel = await _cartItemRepo.DeleteCartItemAsync(id, appUser.Id);
            if (cartItemModel == null) return NotFound("Product does not exist in cart");

            return Ok("Delete successfully");
        }
    }
}