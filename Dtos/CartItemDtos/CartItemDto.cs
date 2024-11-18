namespace ecommerce.Dtos.CartItemDtos
{
    public class CartItemDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public string? UserName { get; set; }
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? CategoryName { get; set; }
    }
}