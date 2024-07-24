namespace ecommerce.Dtos.OrderDtos
{
    public class OrderDto
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string District { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Payment { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal SubToTal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalInvoicement { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsCanceled { get; set; }
        public string? AppUserId { get; set; }
    }
}