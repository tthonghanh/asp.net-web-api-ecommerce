namespace ecommerce.Dtos.ProductImageDtos
{
    public class ProductImageDto
    {
        public string Id { get; set; } = null!;
        public string? ImageName { get; set; }
        public string? ExtensionType { get; set; }
        public string? ProductId { get; set; }
        public string? ImageBase64 { get; set; }
    }
}