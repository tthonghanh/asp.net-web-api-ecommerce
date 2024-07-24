using ecommerce.Dtos.CategoryDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category categoryModel)
        {
            return new CategoryDto
            {
                Id = categoryModel.Id,
                Name = categoryModel.Name,
                ParentCategory = categoryModel.ParentCategory
            };
        }

        public static Category ToCategoryFromCreate(this CreateCategoryRequestDto categoryDto)
        {
            return new Category
            {
                Name = categoryDto.Name,
                ParentCategory = categoryDto.ParentCategory
            };
        }
    }
}