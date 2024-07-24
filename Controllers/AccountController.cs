using ecommerce.Dtos.Account;
using ecommerce.Interfaces;
using ecommerce.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public AccountController(IAccountRepository accountRepo, ITokenService tokenService, IUserService userService)
        {
            _accountRepo = accountRepo;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("CustomerRegister")]
        public async Task<IActionResult> CustomerRegister([FromForm] RegisterDto registerDto) {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var (appUser, error) = await _accountRepo.RegisterAsync(registerDto, "Customer");

                if (appUser == null) return StatusCode(500, error);

                return Ok(new NewUserDto {
                    Name = appUser.UserName!,
                    Email = appUser.Email!,
                    PhoneNumber = appUser.PhoneNumber!,
                    Token = _tokenService.CreateToken(appUser, ["Customer"])
                });
            }
            catch (System.Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("AdminRegister")]
        public async Task<IActionResult> AdminRegister([FromForm] RegisterDto registerDto) {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var (appUser, error) = await _accountRepo.RegisterAsync(registerDto, "Admin");

                if (appUser == null) return StatusCode(500, error);

                return Ok(new NewUserDto {
                    Name = appUser.UserName!,
                    Email = appUser.Email!,
                    PhoneNumber = appUser.PhoneNumber!,
                    Token = _tokenService.CreateToken(appUser, ["Admin"])
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (appUser, roles) = await _accountRepo.LoginAsync(loginDto);

            if (appUser == null || roles == null) return Unauthorized("Username or password not found");

            return Ok(new NewUserDto {
                Name = appUser.UserName!,
                Email = appUser.Email!,
                PhoneNumber = appUser.PhoneNumber!,
                Token = _tokenService.CreateToken(appUser, roles)
            });
        }

        [HttpGet("GetAllCustomers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCustomers() {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var customers = await _accountRepo.GetAllCustomersAsync();

            return Ok(customers.Select(c => c.ToCustomerDto()));
        }

        [HttpPut("UpdateAccount")]
        [Authorize]
        public async Task<IActionResult> UpdateAccount([FromForm] UpdateAppUserRequestDto appUserDto) {
            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            var (appUserModel, roles) = await _accountRepo.UpdateAccountAsync(appUser, appUserDto);

            return Ok(new NewUserDto {
                Name = appUserModel.UserName!,
                Email = appUserModel.Email!,
                PhoneNumber = appUserModel.PhoneNumber!,
                Token = _tokenService.CreateToken(appUser, roles)
            });
        }

        [HttpDelete("DeleteUser")]
        [Authorize]
        public async Task<IActionResult> DeleteUser() {
            var appUser = await _userService.GetUserAsync();
            if (appUser == null) return Unauthorized("User not found");

            await _accountRepo.DeleteUserAsync(appUser);
            return Ok("User deleted");
        }
    }
}