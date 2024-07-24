namespace ecommerce.Dtos.Account
{
    public class NewUserDto
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}