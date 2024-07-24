using ecommerce.Dtos.ProductOrderDtos;

namespace ecommerce.Dtos.OrderDtos
{
    public class OrderDetailDto
    {
        public OrderDto OrderDto { get; set; } = null!;
        public List<ProductOrderDto> ProductList { get; set; } =[];
    }
}