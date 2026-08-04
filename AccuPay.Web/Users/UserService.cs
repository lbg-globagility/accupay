using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Files;
using AccuPay.Web.Files.Services;
using AccuPay.Web.Users.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class UserService
    {
        private readonly UserManager<AspNetUser> _users;
        private readonly UserEmailService _emailService;
        private readonly ICurrentUser _currentUser;
        private readonly IAspNetUserRepository _repository;
        private readonly GenerateDefaultUserImageService _generateDefaultUserImageService;
        private readonly IFilesystem _filesystem;
        private readonly IFileRepository _fileRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public UserService(
            UserManager<AspNetUser> users,
            UserEmailService emailService,
            ICurrentUser currentUser,
            IAspNetUserRepository repository,
            GenerateDefaultUserImageService generateDefaultUserImageService,
            IFilesystem filesystem,
            IFileRepository fileRepository,
            IEmployeeRepository employeeRepository)
        {
            _users = users;
            _emailService = emailService;
            _currentUser = currentUser;
            _repository = repository;
            _generateDefaultUserImageService = generateDefaultUserImageService;
            _filesystem = filesystem;
            _fileRepository = fileRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<ActionResult<PaginatedList<UserDto>>> List(PageOptions options, string term)
        {
            var (users, count) = await _repository.List(options, _currentUser.ClientId, term);

            var dtos = users.Select(t =>
                new UserDto()
                {
                    Id = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    Email = t.Email,
                    EmployeeId = t.EmployeeId,
                }
            );

            return new PaginatedList<UserDto>(dtos, count, 1, 1);
        }

        public async Task<UserDto> Create(CreateUserDto dto)
        {
            var user = new AspNetUser()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                ClientId = _currentUser.ClientId,
                EmployeeId = dto.EmployeeId,
                CreatedById = _currentUser.UserId
            };
            var result = await _users.CreateAsync(user, GenerateRandomPassword());
            if (result.Succeeded)
            {
                await _emailService.SendInvitation(user);

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
                if (result.Errors?.Count() > 0)
                {
                    throw new BusinessLogicException(result.Errors.ToList()[0].Description);
                }

                throw new BusinessLogicException("Error creating user.");
            }
        }

        public async Task<UserDto> Update(int id, UpdateUserDto dto)
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
                if (result?.Errors?.Count() > 0)
                {
                    throw new BusinessLogicException(result.Errors.ToList()[0].Description);
                }

                throw new BusinessLogicException("Error creating user.");
            }
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _users.FindByIdAsync(id.ToString());

            var employee = user.EmployeeId.HasValue
                ? await _employeeRepository.GetByIdAsync(user.EmployeeId.Value)
                : null;

            var dto = new UserDto()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmployeeId = user.EmployeeId
            };

            return dto;
        }

        public async Task GenerateUserImages()
        {
            var users = await _repository.GetUsersWithoutImageAsync();

            foreach (var user in users)
            {
                user.OriginalImage = await CreateOriginalImageIdAsync(user);

                await _users.UpdateAsync(user);
            }
        }

        public async Task<File> CreateOriginalImageIdAsync(AspNetUser user)
        {
            using var virtualFile = _generateDefaultUserImageService.Create(user);
            var path = $"User/{user.Id}/{virtualFile.Filename}";

            await _filesystem.Move(virtualFile.Stream, path);

            var file = new File(
                key: virtualFile.Filename,
                path: path,
                filename: virtualFile.Filename,
                mediaType: "image/jpeg",
                size: virtualFile.Size);

            //file.CreatedById = _currentUser.UserId;
            //file.UpdatedById = file.CreatedById;

            await _fileRepository.Create(file);

            return file;
        }
        /// <summary>
        /// Generates a random password used only to satisfy Identity's account creation
        /// requirements. It is never logged or returned to the caller — the account stays
        /// unusable until the user sets their own password via the invitation link.
        /// </summary>
        private static string GenerateRandomPassword()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task ResendInvitation(int id)
        {
            var user = await _users.FindByIdAsync(id.ToString());
            if (user == null)
                throw new BusinessLogicException("User not found!");
            if (user.Status != AspNetUserStatus.Pending)
                throw new BusinessLogicException("This account is already verified");
            await _emailService.SendInvitation(user);
        }
    }
}
