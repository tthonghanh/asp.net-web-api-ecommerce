using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProductImageRepository _productImageRepo;
        public ProductImageController(IProductImageRepository productImageRepo)
        {
            _productImageRepo = productImageRepo;
        }
        [HttpGet("GetAllProductImagesByProductId/{productId}")]
        public async Task<IActionResult> GetProductImagesByProductId([FromRoute] string productId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var images = await _productImageRepo.GetProductImagesByProductIdAsync(productId);
            if (images == null) return NotFound("Product Image Not Found");

            return Ok(images.Select(image => image.ToProductImageDto()));
        }

        [HttpGet("GetImageByImageId/{id}")]
        public async Task<IActionResult> GetImageByImageId([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var image = await _productImageRepo.GetImageByImageIdAsync(id);
            if (image == null || image.Data == null) return NotFound("Image Not Found");

            return File(image.Data, "image/png");
        }

        [HttpGet("DownloadImageById/{id}")]
        public async Task<IActionResult> DownloadImageById([FromRoute] string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var fileImage = await _productImageRepo.DownloadImageAsync(id);
            if (fileImage == null) return NotFound("No image");

            return fileImage;
        }

        [HttpPost("UploadProductImage/{productId}")]
        public async Task<IActionResult> UploadImage([FromRoute] string productId, [FromForm] IFormFile[] ImageFiles)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (ImageFiles == null) return NotFound("No image uploaded");

            var (imageModel, success, message) = await _productImageRepo.UploadProductImageAsync(productId, ImageFiles);
            if (imageModel == null || !success) return BadRequest(message);

            return Ok(imageModel.Select(i => i.ToProductImageDto()));
        }

        [HttpPut("UpdateProductImage/{id}")]
        public async Task<IActionResult> UpdateProductImageById([FromRoute] string id, IFormFile ImageFile)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var imageModel = await _productImageRepo.UpdateProductImageByIdAsync(id, ImageFile);
            if (imageModel == null) return NotFound("Image Not Found");

            return Ok(imageModel.ToProductImageDto());
        }

        [HttpDelete("DeleteImageById/{id}")]
        public async Task<IActionResult> DeleteImageById(string id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var imageModel = await _productImageRepo.DeleteProductImageByIdAsync(id);
            if (imageModel == null) return NotFound("Image Not Found");

            return Ok("Delete successfully");
        }
    }
}