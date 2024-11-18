namespace ecommerce.Dtos.ProductDtos
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal ActualPrice { get; set; }
        public string? Discription { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal? Stars { get; set; }
        public int SaleCount { get; set; }
    }
}