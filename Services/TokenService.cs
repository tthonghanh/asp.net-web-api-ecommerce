using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ecommerce.Interfaces;
using ecommerce.Models;
using Microsoft.IdentityModel.Tokens;

namespace ecommerce.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:SigningKey"]!));
        }
        public string CreateToken(AppUser appUser, IList<string> roles)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName!),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email!),
                new Claim(ClaimTypes.MobilePhone, appUser.PhoneNumber!),
            };

            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(3),
                SigningCredentials = credentials,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}