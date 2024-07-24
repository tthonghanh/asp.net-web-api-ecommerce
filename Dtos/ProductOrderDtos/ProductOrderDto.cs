namespace ecommerce.Dtos.ProductOrderDtos
{
    public class ProductOrderDto
    {
        public string Id { get; set; } = null!;
        public string? OrderId { get; set; }
        public string? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}