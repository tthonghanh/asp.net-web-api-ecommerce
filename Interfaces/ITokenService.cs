using ecommerce.Models;

namespace ecommerce.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser, IList<string> roles);
    }
}