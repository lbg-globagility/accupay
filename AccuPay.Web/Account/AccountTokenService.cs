using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccuPay.Web.Account
{
    public class AccountTokenService
    {
        private const int TokenExpirationHours = 24;

        private readonly TokenService _tokenService;
        private readonly UserDataService _userDataService;
        private readonly UserRepository _userRepository;

        public AccountTokenService(TokenService tokenService, UserDataService userDataService, UserRepository userRepository)
        {
            _tokenService = tokenService;
            _userDataService = userDataService;
            _userRepository = userRepository;
        }

        public ClaimsPrincipal Decode(string encodedToken)
        {
            return _tokenService.Decode(encodedToken);
        }

        public async Task<string> CreateAccessToken(AspNetUser user, Organization organization)
        {
            int? desktopUserId = await GetDesktopUserId(user);

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(CustomClaimTypes.CompanyId, organization.RowID.ToString()),
                new Claim(CustomClaimTypes.ClientId, user.ClientId.ToString()),
                new Claim(CustomClaimTypes.DesktopUserId, desktopUserId.ToString())
            };

            return _tokenService.Encode(
                timeToExpire: TimeSpan.FromHours(TokenExpirationHours),
                customClaims: claims);
        }

        private async Task<int?> GetDesktopUserId(AspNetUser user)
        {
            var desktopUser = await _userRepository.GetByAspNetUserIdAsync(user.Id);

            var desktopUserId = desktopUser?.RowID;

            if (desktopUserId == null)
            {
                var savedByUser = await _userRepository.GetFirstUserAsync();
                await _userDataService.CreateAsync(user, savedByUser.RowID.Value);

                var newUser = await _userRepository.GetByAspNetUserIdAsync(user.Id);

                desktopUserId = newUser.RowID;
            }

            return desktopUserId;
        }
    }
}
