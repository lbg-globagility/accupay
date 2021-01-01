using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
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

        public async Task CreateAsync(AspNetUser user, bool isEncrypted = false)
        {
            await SanitizeEntity(user, isEncrypted);

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(AspNetUser user, bool isEncrypted = false)
        {
            await SanitizeEntity(user, isEncrypted);

            await _userRepository.UpdateAsync(user);
        }

        private async Task SanitizeEntity(AspNetUser user, bool isEncrypted)
        {
            if (user == null)
                throw new BusinessLogicException("Invalid user.");

            if (user.ClientId == 0)
                throw new BusinessLogicException("Client is required.");

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

        public async Task SoftDeleteAsync(int id, int deletedByUserId, int clientId)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                throw new BusinessLogicException("User does not exists.");

            if (id == deletedByUserId)
                throw new BusinessLogicException("Cannot delete own account.");

            if (user.DeletedAt != null)
                return;

            if ((await _userRepository.List(PageOptions.AllData, clientId)).users.Count == 1)
                throw new BusinessLogicException("Cannot delete user. There should be at least one user.");

            await _userRepository.SoftDeleteAsync(user);
        }
    }
}