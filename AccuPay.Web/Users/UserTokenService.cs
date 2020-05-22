using AccuPay.Data.Entities;
using AccuPay.Web.Core.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Users
{
    public class UserTokenService
    {
        private readonly TokenService _tokenService;

        public UserTokenService(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public string CreateRegistrationToken(AspNetUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var token = _tokenService.Encode(TimeSpan.FromDays(7), claims);

            return token;
        }
    }
}
