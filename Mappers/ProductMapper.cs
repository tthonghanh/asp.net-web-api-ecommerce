using ecommerce.Dtos.ProductDtos;
using ecommerce.Extensions;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class ProductMapper
    {
        public static ProductDto ToProductDto(this Product productModel)
        {
            return new ProductDto
            {
                Id = productModel.Id,
                Name = productModel.Name,
                OriginalPrice = productModel.OriginalPrice,
                ActualPrice = productModel.ActualPrice,
                Discription = productModel.Discription,
                CreateAt = productModel.CreateAt,
                CategoryId = productModel.CategoryId,
                Stars = productModel.GetAverageFeedbackStars(),
            };
        }

        public static Product ToProductFromCreate(this CreateProductRequestDto productDto)
        {
            return new Product
            {
                Name = productDto.Name,
                OriginalPrice = productDto.OriginalPrice,
                ActualPrice = productDto.ActualPrice,
                Discription = productDto.Discription,
                CategoryId = productDto.CategoryId,
            };
        }

        public static ProductDetailDto ToProductDetailDto(this Product productModel) {
            return new ProductDetailDto {
                Id = productModel.Id,
                Name = productModel.Name,
                OriginalPrice = productModel.OriginalPrice,
                ActualPrice = productModel.ActualPrice,
                Discription = productModel.Discription,
                CreateAt = productModel.CreateAt,
                CategoryId = productModel.CategoryId,
                CategoryName = productModel.Category.Name,
                Stars = productModel.GetAverageFeedbackStars(),
                ProductImages = productModel.ProductImages
                                            .Select(image => image.ToProductImageDto())
                                            .ToList(),
                Feedbacks = productModel.Feedbacks
                                        .Select(feedback => feedback.ToFeedbackDto())
                                        .ToList()
            };
        }
    }
}