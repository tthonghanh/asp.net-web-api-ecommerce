using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IUserService
    {
        Task<AppUser?> GetUserAsync();
    }
}