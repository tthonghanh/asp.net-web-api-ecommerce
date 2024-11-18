namespace ecommerce.Dtos.CategoryDtos
{
    public class CategoryDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ParentCategory { get; set; }
    }
}