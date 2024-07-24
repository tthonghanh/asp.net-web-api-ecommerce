using ecommerce.Dtos.Account;
using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface IAccountRepository
    {
        Task<(AppUser?, object?)> RegisterAsync(RegisterDto registerDto, string role);
        Task<(AppUser?, IList<string>?)> LoginAsync(LoginDto loginDto);
        Task<IList<AppUser>> GetAllCustomersAsync();
        Task<AppUser> DeleteUserAsync(AppUser appUser);
        Task<(AppUser appUserModel, IList<string> roles)> UpdateAccountAsync(AppUser appUser, UpdateAppUserRequestDto appUserDto);
    }
}