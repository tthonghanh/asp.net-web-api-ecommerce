using ecommerce.Dtos.Account;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountRepository(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<AppUser> DeleteUserAsync(AppUser appUser)
        {
            await _userManager.DeleteAsync(appUser);
            return appUser;
        }

        public Task<IList<AppUser>> GetAllCustomersAsync()
        {
            return _userManager.GetUsersInRoleAsync("Customer");
        }

        public async Task<(AppUser?, IList<string>?)> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null) return (null, null);

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            var role = await _userManager.GetRolesAsync(user);

            if (!result.Succeeded) return (null, null);

            return (user, role);
        }

        public async Task<(AppUser?, object?)> RegisterAsync(RegisterDto registerDto, string role)
        {
            var appUser = new AppUser {
                UserName = registerDto.Name,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded) {
                var roleResult = await _userManager.AddToRoleAsync(appUser, role);

                if (roleResult.Succeeded) {
                    return (appUser, null);
                }
                else return (null, roleResult.Errors);
            }
            else return (null, createdUser.Errors);
        }

        public async Task<(AppUser appUserModel, IList<string> roles)> UpdateAccountAsync(AppUser appUser, UpdateAppUserRequestDto appUserDto)
        {
            var roles = await _userManager.GetRolesAsync(appUser);

            appUser.UserName = appUserDto.Name;
            appUser.Email = appUserDto.Email;
            appUser.PhoneNumber = appUserDto.PhoneNumber;

            await _userManager.UpdateAsync(appUser);
            return (appUser, roles);
        }
    }
}