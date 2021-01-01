using AccuPay.Core.Entities;
using AccuPay.Web.Core.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Account
{
    public class AccountTokenService
    {
        private const int TokenExpirationHours = 24;

        private readonly TokenService _tokenService;

        public AccountTokenService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public ClaimsPrincipal Decode(string encodedToken)
        {
            return _tokenService.Decode(encodedToken);
        }

        public string CreateAccessToken(AspNetUser user, Organization organization)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(CustomClaimTypes.CompanyId, organization.RowID.ToString()),
                new Claim(CustomClaimTypes.ClientId, user.ClientId.ToString()),
                new Claim(CustomClaimTypes.EmployeeId, user.EmployeeId.HasValue ? user.EmployeeId.ToString() : string.Empty)
            };

            return _tokenService.Encode(
                timeToExpire: TimeSpan.FromHours(TokenExpirationHours),
                customClaims: claims);
        }
    }
}
