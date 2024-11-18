using ecommerce.Dtos.ProductDtos;
using ecommerce.Helpers;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllProductsAsync(QueryObject query);
        Task<List<ProductDto>?> GetAllProductsByCategoryIdAsync(string id, QueryObject query);
        Task<ProductDetailDto?> GetProductByIdAsync(string id);
        Task<int?> GetSalesCount(string productId);
        Task CreateProductAsync(CreateProductRequestDto productDto);
        Task<int> UpdateProductAsync(string id, UpdateProductRequestDto productDto);
        Task DeleteProductAsync(string id);
        Task<bool> ProductExist(string id);
    }
}