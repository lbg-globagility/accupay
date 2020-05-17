using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Accupay.Web.Core.Auth
{
    /// <summary>
    /// Represents the existing user for the current request.
    /// </summary>
    public class CurrentUser
    {
        public Guid Id { get; private set; }

        public int OrganizationId { get; private set; }

        public CurrentUser(IHttpContextAccessor accessor)
        {
            ReadClaims(accessor?.HttpContext?.User);
        }

        private void ReadClaims(ClaimsPrincipal user)
        {
            if (user is null)
            {
                return;
            }

            ReadUserId(user);
            ReadCompanyId(user);
        }

        private void ReadUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            Id = (value is null ? Guid.Empty : Guid.Parse(value));
        }

        private void ReadCompanyId(ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(CustomClaimTypes.CompanyId);

            OrganizationId = claim is null ? 0 : int.Parse(claim.Value);
        }
    }
}
