namespace ecommerce.Dtos.CategoryDtos
{
    public class CategoryDto
    {
        public string Id { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string ParentCategory { get; set; } = string.Empty;
    }
}