using ecommerce.Dtos.FeedbackDtos;
using ecommerce.Dtos.ProductImageDtos;

namespace ecommerce.Dtos.ProductDtos
{
    public class ProductDetailDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public string? Discription { get; set; }
        public DateTime CreateAt { get; set; }
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Stars { get; set; }
        public int SaleCount { get; set; }
        public List<ProductImageDto> ProductImages { get; set; } = [];
        public List<FeedbackDto> Feedbacks { get; set; } = [];
    }
}