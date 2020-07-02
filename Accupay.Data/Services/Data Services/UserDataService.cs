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
        private readonly UserRepository _userRepository;
        private readonly PositionRepository _positionRepository;
        private readonly PositionViewRepository _positionViewRepository;
        private readonly IEncryption _encryption;

        public UserDataService(
            UserRepository userRepository,
            PositionRepository positionRepository,
            PositionViewRepository positionViewRepository,
            IEncryption encryption)
        {
            _userRepository = userRepository;
            _positionRepository = positionRepository;
            _positionViewRepository = positionViewRepository;
            _encryption = encryption;
        }

        public async Task CreateAsync(AspNetUser aspNetUser, int savedByUserId)
        {
            var position = await _positionRepository.GetFirstPositionAsync();

            if (position?.RowID == null)
            {
                // BusinessLogicException is not used here because we don't want the users to read this error.
                // At least one position should be added for every database or
                // create a default position if there is no one.
                throw new Exception("No default position.");
            }

            var username = await _userRepository.GetUniqueUsernameAsync(aspNetUser.UserName, aspNetUser.ClientId);

            var user = User.NewUser(organizationId: position.OrganizationID.Value, userId: savedByUserId);

            user.FirstName = aspNetUser.FirstName;
            user.LastName = aspNetUser.LastName;
            user.EmailAddress = aspNetUser.Email;

            user.PositionID = position.RowID.Value;

            user.Username = username;
            user.Password = username;

            user.AspNetUserId = aspNetUser.Id;
            await CreateAsync(user);
        }

        public async Task CreateAsync(User user)
        {
            await SanitizeEntity(user);

            await _positionViewRepository.FillUserPositionViewAsync(
                positionId: user.PositionID,
                organizationId: user.OrganizationID,
                savedByUserId: user.CreatedBy.Value);

            await _userRepository.CreateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            await SanitizeEntity(user);

            await _positionViewRepository.FillUserPositionViewAsync(
                positionId: user.PositionID,
                organizationId: user.OrganizationID,
                savedByUserId: user.LastUpdBy.Value);

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
                throw new BusinessLogicException("Password is required.");

            var existingUsername = await _userRepository.GetByUsernameAsync(user.Username);
            if (existingUsername != null && existingUsername.RowID != user.RowID)
                throw new BusinessLogicException("Password already exists.");

            user.Username = _encryption.Encrypt(user.Username);
            user.Password = _encryption.Encrypt(user.Password);
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