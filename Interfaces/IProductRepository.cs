using ecommerce.Dtos.ProductDtos;
using ecommerce.Helpers;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync(QueryObject query);
        Task<List<Product>?> GetAllProductsByCategoryIdAsync(string id, QueryObject query);
        Task<Product?> GetProductByIdAsync(string id);
        Task<int?> GetSalesCount(string productId);
        Task<(Product product, bool succeed, string message)> CreateProductAsync(CreateProductRequestDto productDto);
        Task<Product?> UpdateProductAsync(string id, UpdateProductRequestDto productDto);
        Task<Product?> DeleteProductAsync(string id);
        Task<bool> ProductExist(string id);
    }
}