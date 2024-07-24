using System.Security.Claims;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace ecommerce.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public UserService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }
        public async Task<AppUser?> GetUserAsync()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.GivenName);
            if (userName == null) return null;

            var appUser = await _userManager.FindByNameAsync(userName);
            if (appUser == null) return null;

            return appUser;
        }
    }
}