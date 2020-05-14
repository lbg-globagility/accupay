using AccuPay.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class UserService
    {
        private readonly UserManager<AspNetUser> _users;

        public UserService(UserManager<AspNetUser> users)
        {
            _users = users;
        }

        public async Task<UserDto> Create(CreateUserDto dto)
        {
            var user = new AspNetUser()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = await _users.CreateAsync(user);
            if (result.Succeeded)
            {
                return new UserDto()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
            }
            else
            {
                // Probably not the best way to return errors
                throw new Exception();
            }
        }
    }
}
