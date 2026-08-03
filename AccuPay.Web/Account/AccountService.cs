using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Infrastructure.Data;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
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
        private readonly IUserDataService _userDataService;
        private readonly IFilesystem _filesystem;
        private readonly IFileRepository _fileRepository;



        public AccountService(
            UserManager<AspNetUser> users,
            SignInManager<AspNetUser> signIn,
            AccountTokenService accountTokenService,
            UserTokenService userTokenService,
            IOrganizationRepository organizationRepository,
            ICurrentUser currentUser,
            IUserDataService userDataService,
            IFilesystem filesystem,
            IFileRepository fileRepository)
        {
            _users = users;
            _signIn = signIn;
            _accountTokenService = accountTokenService;
            _userTokenService = userTokenService;
            _organizationRepository = organizationRepository;
            _currentUser = currentUser;
            _userDataService = userDataService;
            _filesystem = filesystem;
            _fileRepository = fileRepository;
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
                Type = user.EmployeeId.HasValue ? "Employee" : "Admin",
                Image = await GetImageBase64(user.OriginalImageId)
            };

            return userDto;
        }

        private async Task<string> GetImageBase64(int? imageId)
        {
            if (!imageId.HasValue)
            {
                return null;
            }

            var file = await _fileRepository.GetById(imageId.Value);

            if (file == null)
            {
                return null;
            }

            using var stream = await _filesystem.Get(file.Path);
            using var memoryStream = new System.IO.MemoryStream();
            await stream.CopyToAsync(memoryStream);

            return $"data:{file.MediaType};base64,{Convert.ToBase64String(memoryStream.ToArray())}";
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
        public async Task<UserDto> ChangePassword(string oldpassword,string password)
        {
            var user = await _users.FindByIdAsync(_currentUser.UserId.ToString());

            var result = await _users.ChangePasswordAsync(user, oldpassword, password);

            if (!result.Succeeded)
            {
                throw new BusinessLogicException(result.Errors.ToList()[0].Description);
            }
            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return userDto;

        }
        public async Task<UserDto> ChangeUserPassword(int userId, string password)
        {
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                throw new BusinessLogicException("User not found.");
            }

            var token = await _users.GeneratePasswordResetTokenAsync(user);
            var result = await _users.ResetPasswordAsync(user, token, password);

            if (!result.Succeeded)
            {
                throw new BusinessLogicException(result.Errors.First().Description);
            }

            var userDto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return userDto;

        }
        public async Task UpdateCurrentUserImage(IFormFile image)
        {
            if (image == null)
            {
                throw new BusinessLogicException("Image is required.");
            }

            var user = await _users.FindByIdAsync(_currentUser.UserId.ToString());

            var path = $"User/{user.Id}/{image.FileName}";
            var savedPath = await _filesystem.Move(image, path);

            if (user.OriginalImageId.HasValue)
            {
                var file = await _fileRepository.GetById(user.OriginalImageId.Value);

                file.Key = image.FileName;
                file.Filename = image.FileName;
                file.Path = savedPath;
                file.MediaType = image.ContentType;
                file.Size = image.Length;
                file.UpdatedById = _currentUser.UserId;

                await _fileRepository.Update(file);
            }
            else
            {
                var file = new File(
                    key: image.FileName,
                    location: savedPath,
                    file: image);

                file.CreatedById = _currentUser.UserId;
                file.UpdatedById = file.CreatedById;

                await _fileRepository.Create(file);

                user.OriginalImage = file;

                await _users.UpdateAsync(user);
            }
        }
    }
}
