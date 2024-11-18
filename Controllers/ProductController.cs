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

            return Ok(products);
        }

        [HttpGet("GetAllProductsByCategoryId/{id}")]
        public async Task<IActionResult> GetAllProductsByCategoryId([FromRoute] string id, [FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var products = await _productRepo.GetAllProductsByCategoryIdAsync(id, query);

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("GetProductByProductId/{id}")]
        public async Task<IActionResult> GetProductByProductId([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null) return NotFound("Product not found");

            return Ok(product);
        }

        [HttpPost("CreateProduct")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequestDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _categoryRepo.CategoryExist(productDto.CategoryId)) return NotFound("Category not found");

            await _productRepo.CreateProductAsync(productDto);

            // if (!createdProduct.succeed) return StatusCode(500, createdProduct.message);

            return Created();
        }

        [HttpPut("UpdateProduct/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProductById([FromRoute] string id, [FromForm] UpdateProductRequestDto productDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _categoryRepo.CategoryExist(productDto.CategoryId!)) return NotFound("Category not found");

            try
            {
                var statusCode = await _productRepo.UpdateProductAsync(id, productDto);

                if (statusCode == 0) return NotFound("Product does not exist");

                return Ok("Product updated");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("DeleteProductById/{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProductById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await _productRepo.DeleteProductAsync(id);
                return Ok("Product Deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}