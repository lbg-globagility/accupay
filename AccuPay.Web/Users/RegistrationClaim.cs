using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Users
{
    public class RegistrationClaim
    {
        public string UserId { get; private set; }

        public RegistrationClaim(ClaimsPrincipal principal)
        {
            UserId = ReadClaim(principal, JwtRegisteredClaimNames.Sub);
        }

        private string ReadClaim(ClaimsPrincipal principal, string claimType)
        {
            var claim = principal.FindFirst(claimType);

            if (claim is null)
            {
                throw new Exception($"Claim `{claimType}` is missing from token");
            }

            return claim.Value;
        }
    }
}
