using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Interfaces;
using AccuPay.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class UserDataService
    {
        private readonly AspNetUserRepository _userRepository;
        private readonly IEncryption _encryption;

        public UserDataService(
            AspNetUserRepository userRepository,
            IEncryption encryption)
        {
            _userRepository = userRepository;
            _encryption = encryption;
        }

        public async Task CreateAsync(AspNetUser user)
        {
            await SanitizeEntity(user);

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(AspNetUser user)
        {
            await SanitizeEntity(user);

            await _userRepository.UpdateAsync(user);
        }

        private async Task SanitizeEntity(AspNetUser user, bool isEncrypted = true)
        {
            if (user == null)
                throw new BusinessLogicException("Invalid user.");

            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new BusinessLogicException("First name is required.");

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new BusinessLogicException("Last name is required.");

            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new BusinessLogicException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.DesktopPassword))
                throw new BusinessLogicException("Password is required.");

            if (!isEncrypted)
            {
                user.DesktopPassword = _encryption.Encrypt(user.DesktopPassword);
            }

            var existingUserName = await _userRepository.GetByUserNameAsync(user.UserName);
            if (existingUserName != null && existingUserName.Id != user.Id)
                throw new BusinessLogicException("Username is already used by an existing or a deleted user.");

            user.Email = user.Email?.Trim();
            user.NormalizedEmail = user.Email?.ToUpper();

            user.UserName = user.UserName.Trim();
            user.NormalizedUserName = user.UserName.ToUpper();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new BusinessLogicException("User does not exists.");

            await _userRepository.SoftDeleteAsync(user);
        }
    }
}