using AccuPay.Data.Entities;
using AccuPay.Web.Core.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Account
{
    public class AccountTokenService
    {
        private readonly TokenService _tokenService;

        public AccountTokenService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public ClaimsPrincipal Decode(string encodedToken)
        {
            return _tokenService.Decode(encodedToken);
        }

        public string CreateAccessToken(AspNetUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(CustomClaimTypes.CompanyId, user.OrganizationId.ToString())
            };

            return _tokenService.Encode(
                timeToExpire: TimeSpan.FromHours(24),
                customClaims: claims);
        }
    }
}
