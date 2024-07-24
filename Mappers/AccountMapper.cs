using ecommerce.Dtos.Account;
using ecommerce.Models;

namespace ecommerce.Mappers
{
    public static class AccountMapper
    {
        public static CustomerDto ToCustomerDto(this AppUser appUser) {
            return new CustomerDto {
                Id = appUser.Id,
                Name = appUser.UserName!,
                Email = appUser.Email!,
                PhoneNumber = appUser.PhoneNumber!,
            };
        }
    }
}