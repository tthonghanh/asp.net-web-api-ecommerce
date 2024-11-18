using ecommerce.Dtos.ProductImageDtos;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class ProductImageMapper
    {
        public static ProductImageDto ToProductImageDto(this ProductImage productImageModel)
        {
            return new ProductImageDto
            {
                Id = productImageModel.Id,
                ImageName = productImageModel.ImageName,
                ImageBase64 = Convert.ToBase64String(productImageModel.Data)
            };
        }
    }
}