using AccuPay.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class UserService
    {
        /// <summary>
        /// Needs to be replaced later on with a real password
        /// </summary>
        private readonly string DEFAULT_PASSWORD = "password";

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

            var result = await _users.CreateAsync(user, DEFAULT_PASSWORD);
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

        public async Task<UserDto> Update(Guid id, UpdateUserDto dto)
        {
            var user = await _users.FindByIdAsync(id.ToString());

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;

            var result = await _users.UpdateAsync(user);
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
