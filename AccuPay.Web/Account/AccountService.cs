using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Account
{
    public class AccountService
    {
        private readonly UserManager<AspNetUser> _users;
        private readonly SignInManager<AspNetUser> _signIn;
        private readonly AccountTokenService _accountTokenService;
        private readonly UserTokenService _userTokenService;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly ICurrentUser _currentUser;

        public AccountService(
            UserManager<AspNetUser> users,
            SignInManager<AspNetUser> signIn,
            AccountTokenService accountTokenService,
            UserTokenService userTokenService,
            IOrganizationRepository organizationRepository,
            ICurrentUser currentUser)
        {
            _users = users;
            _signIn = signIn;
            _accountTokenService = accountTokenService;
            _userTokenService = userTokenService;
            _organizationRepository = organizationRepository;
            _currentUser = currentUser;
        }

        public async Task<string> Login(string username, string password)
        {
            var user = await _users.FindByNameAsync(username);

            if (user == null)
            {
                throw LoginException.CredentialsMismatch();
            }

            var result = await _signIn.PasswordSignInAsync(user, password, false, false);

            if (!result.Succeeded)
            {
                throw LoginException.CredentialsMismatch();
            }

            var organization = await _organizationRepository.GetFirst(user.ClientId);

            if (organization is null)
            {
                throw LoginException.NoOrganization();
            }

            var token = _accountTokenService.CreateAccessToken(user, organization);

            return token;
        }

        public async Task<string> ChangeOrganization(int organizationId)
        {
            var user = await _users.FindByIdAsync(_currentUser.UserId.ToString());
            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            if (organization.ClientId != _currentUser.ClientId)
            {
                throw new Exception("User has no permission to acess organization");
            }

            var token = _accountTokenService.CreateAccessToken(user, organization);

            return token;
        }

        public async Task<RegisterDto> Verify(string userId)
        {
            var user = await _users.FindByIdAsync(userId);

            if (user is null)
            {
                throw new Exception("Cannot find user account");
            }

            if (user.Status != AspNetUserStatus.Pending)
            {
                throw new Exception("User account already verified");
            }

            var registerDto = new RegisterDto()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return registerDto;
        }

        public async Task<UserDto> GetInformation()
        {
            var user = await _users.FindByIdAsync(_currentUser.UserId.ToString());

            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Type = user.EmployeeId.HasValue ? "Employee" : "Admin"
            };

            return userDto;
        }

        public async Task<UserDto> Register(VerifyRegistrationDto dto)
        {
            var claims = _userTokenService.DecodeRegistrationToken(dto.Token);
            var user = await _users.FindByIdAsync(claims.UserId);

            if (user.Status != AspNetUserStatus.Pending)
            {
                throw new Exception("User account already verified");
            }

            var passwordToken = await _users.GeneratePasswordResetTokenAsync(user);
            var result = await _users.ResetPasswordAsync(user, passwordToken, dto.Password);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to change password");
            }

            user.Status = AspNetUserStatus.Verified;
            await _users.UpdateAsync(user);

            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return userDto;
        }
    }
}
