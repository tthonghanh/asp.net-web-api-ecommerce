using ecommerce.Dtos.ProductDtos;
using ecommerce.Helpers;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _productImageRepo;
        private readonly ICategoryRepository _categoryRepo;
        public ProductController(IProductRepository productRepo, IProductImageRepository productImageRepo, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _productImageRepo = productImageRepo;
            _categoryRepo = categoryRepo;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProduct([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var products = await _productRepo.GetAllProductsAsync(query);

            return Ok(products.Select(p => p.ToProductDto()));
        }

        [HttpGet("GetAllProductsByCategoryId/{id}")]
        public async Task<IActionResult> GetAllProductsByCategoryId([FromRoute] string id, [FromQuery] QueryObject query) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var products = await _productRepo.GetAllProductsByCategoryIdAsync(id, query);
            if (products == null) return NotFound("Category not found");

            return Ok(products.Select(p => p.ToProductDto()));
        }

        [HttpGet("GetProductByProductId/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product not found");

            return Ok(product.ToProductDetailDto());
        }

        [HttpGet("GetSalesCount/{productId}")]
        public async Task<IActionResult> GetSalesCount([FromRoute] string productId) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var count = await _productRepo.GetSalesCount(productId);
            if (count == null) return NotFound("Product not found");

            return Ok(count);
        }

        [HttpPost("CreateProduct")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _categoryRepo.CategoryExist(productDto.CategoryId!)) return NotFound("Category not found");

            var createdProduct = await _productRepo.CreateProductAsync(productDto);

            if (!createdProduct.succeed) return StatusCode(500, createdProduct.message);

            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.product.Id }, createdProduct.product.ToProductDto());
        }

        [HttpPut("UpdateProduct/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductById([FromRoute] string id, [FromForm] UpdateProductRequestDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _categoryRepo.CategoryExist(productDto.CategoryId!)) return NotFound("Category not found");

            var productModel = await _productRepo.UpdateProductAsync(id, productDto);
            if (productModel == null) return NotFound("Product Not Found");

            return Ok(productModel.ToProductDto());
        }

        [HttpDelete("DeleteProductById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var productModel = await _productRepo.DeleteProductAsync(id);
            if (productModel == null) return NotFound("Product Not Found");

            return Ok("Delete successfully");
        }
    }
}