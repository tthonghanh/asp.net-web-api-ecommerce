using ecommerce.Dtos.ProductDtos;
using ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Interfaces
{
    public interface IProductImageRepository
    {
        Task<(List<ProductImage>? productImages, bool succeed, string message)> UploadProductImageAsync(string productId, IFormFile[] imageFiles);
        Task<(List<ProductImage>? productImages, bool succeed, string message)> UploadProductImageFromProductCreateAsync(string productId, CreateProductRequestDto productDto);
        Task<List<ProductImage>?> GetProductImagesByProductIdAsync(string productId);
        Task<ProductImage?> GetImageByImageIdAsync(string id);
        Task<FileContentResult?> DownloadImageAsync(string id);
        Task<ProductImage?> UpdateProductImageByIdAsync(string id, IFormFile productDto);
        Task<ProductImage?> DeleteProductImageByIdAsync(string id);
    }
}