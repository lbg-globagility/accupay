using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class UserDataService
    {
        private readonly UserRepository _userRepository;
        private readonly PositionViewRepository _positionViewRepository;

        public UserDataService(UserRepository userRepository, PositionViewRepository positionViewRepository)
        {
            _userRepository = userRepository;
            _positionViewRepository = positionViewRepository;
        }

        public async Task CreateAsync(User user)
        {
            await SanitizeEntity(user);

            await _positionViewRepository.FillUserPositionViewAsync(
                positionId: user.PositionID,
                organizationId: user.OrganizationID,
                userId: user.CreatedBy.Value);

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await SanitizeEntity(user);

            await _positionViewRepository.FillUserPositionViewAsync(
                positionId: user.PositionID,
                organizationId: user.OrganizationID,
                userId: user.LastUpdBy.Value);

            await _userRepository.UpdateAsync(user);
        }

        private async Task SanitizeEntity(User user)
        {
            if (user == null)
                throw new BusinessLogicException("Invalid user.");

            if (string.IsNullOrWhiteSpace(user.FirstName))
                throw new BusinessLogicException("First name is required.");

            if (string.IsNullOrWhiteSpace(user.LastName))
                throw new BusinessLogicException("Last name is required.");

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new BusinessLogicException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new BusinessLogicException("Username is required.");

            var existingUsername = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUsername != null && existingUsername.RowID != user.RowID)
                throw new BusinessLogicException("Username already exists.");
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