using ecommerce.Data;
using ecommerce.Dtos.ProductDtos;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductImage?> DeleteProductImageByIdAsync(string id)
        {
            var imageModel = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (imageModel == null) return null;

            _context.Remove(imageModel);
            await _context.SaveChangesAsync();

            return imageModel;
        }

        public async Task<FileContentResult?> DownloadImageAsync(string id)
        {
            var imageModel = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);

            if (imageModel == null || imageModel.Data == null) return null;
            var fileContentResult = new FileContentResult(imageModel.Data, "application/png")
            {
                FileDownloadName = $"{imageModel.ImageName}.png"
            };
            return fileContentResult;
        }

        public async Task<ProductImage?> GetImageByImageIdAsync(string id)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<ProductImage>?> GetProductImagesByProductIdAsync(string productId)
        {
            var productModel = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productId);

            if (productModel == null) return null;
            return productModel.ProductImages;
        }

        public async Task<ProductImage?> UpdateProductImageByIdAsync(string id, IFormFile ImageFile)
        {
            var existingImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);
            if (existingImage == null) return null;

            existingImage.ExtensionType = Path.GetExtension(ImageFile.FileName);

            using (var memoryStream = new MemoryStream())
            {
                await ImageFile.CopyToAsync(memoryStream);
                existingImage.Data = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();
            return existingImage;
        }

        public async Task<(List<ProductImage>? productImages, bool succeed, string message)> UploadProductImageAsync(string productId, IFormFile[] imageFiles)
        {
            var productModel = await _context.Products.Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Id == productId);
            if (productModel == null) return (null, false, "Product Not Found");

            if (productModel.ProductImages.Count == 5) return (productModel.ProductImages.ToList(), false, $"You cannot add more images");

            if (productModel.ProductImages.Count + imageFiles.Length > 5) return (productModel.ProductImages.ToList(), false, $"You can only add {5 - productModel.ProductImages.Count} more image(s)");

            const long maxFileSize = 5 * 1024 * 1024;
            foreach (var image in imageFiles)
            {
                if (image.Length > maxFileSize)
                {
                    return (null, false, $"Image size exceeds the maximum allowed size of 5 MB");
                }
            }

            var imageList = new List<ProductImage>();
            foreach (var image in imageFiles)
            {
                var imageModel = new ProductImage
                {
                    ExtensionType = Path.GetExtension(image.FileName),
                    ProductId = productId
                };

                imageModel.ImageName = $"{productModel.Name}_{imageModel.Id}";

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageModel.Data = memoryStream.ToArray();
                }

                await _context.ProductImages.AddAsync(imageModel);
                await _context.SaveChangesAsync();

                imageList.Add(imageModel);
            }
            return (imageList, true, "Images uploaded successfully");
        }

        public async Task<(List<ProductImage>? productImages, bool succeed, string message)> UploadProductImageFromProductCreateAsync(string productId, CreateProductRequestDto productDto)
        {
            if (productDto.ImageFile.Length > 5) return (null, false, $"You cannot add more than 5 images");

            const long maxFileSize = 5 * 1024 * 1024;
            foreach (var image in productDto.ImageFile)
            {
                if (image.Length > maxFileSize)
                {
                    return (null, false, $"Image size exceeds the maximum allowed size of 5 MB");
                }
            }

            var imageList = new List<ProductImage>();
            foreach (var image in productDto.ImageFile)
            {
                var imageModel = new ProductImage
                {
                    ExtensionType = Path.GetExtension(image.FileName),
                    ProductId = productId
                };

                imageModel.ImageName = $"{productDto.Name}_{imageModel.Id}";

                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    imageModel.Data = memoryStream.ToArray();
                }

                await _context.ProductImages.AddAsync(imageModel);
                await _context.SaveChangesAsync();

                imageList.Add(imageModel);
            }
            return (imageList, true, "Images uploaded successfully");
        }
    }
}