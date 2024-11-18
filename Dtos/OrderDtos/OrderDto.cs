namespace ecommerce.Dtos.OrderDtos
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address1 { get; set; }
        public string? Address2 { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Payment { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public decimal SubToTal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalInvoicement { get; set; }
        public string Status { get; set; }
        public bool IsCanceled { get; set; }
        public string? UserName { get; set; }
    }
}