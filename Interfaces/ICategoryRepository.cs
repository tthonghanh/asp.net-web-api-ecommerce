using ecommerce.Dtos.CategoryDtos;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(Category categoryModel);
        Task<Category?> UpdateCategoryAsync(string id, UpdateCategoryRequestDto categoryDto);
        Task<Category?> DeleteCategoryAsync(string id);
        Task<bool> CategoryExist(string? id);
    }
}