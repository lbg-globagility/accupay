using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Core.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public Guid UserId { get; private set; }

        public int OrganizationId { get; private set; }

        public int ClientId { get; private set; }

        public int DesktopUserId { get; private set; }

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
            ReadOrganizationId(user);
            ReadClientId(user);
            ReadDesktopUserId(user);
        }

        private void ReadUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            UserId = (value is null ? Guid.Empty : Guid.Parse(value));
        }

        private void ReadOrganizationId(ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(CustomClaimTypes.CompanyId);

            OrganizationId = claim is null ? 0 : int.Parse(claim.Value);
        }

        private void ReadClientId(ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(CustomClaimTypes.ClientId);

            ClientId = claim is null ? 0 : int.Parse(claim.Value);
        }

        private void ReadDesktopUserId(ClaimsPrincipal principal)
        {
            var claim = principal.FindFirst(CustomClaimTypes.DesktopUserId);

            DesktopUserId = claim is null ? 0 : int.Parse(claim.Value);
        }
    }
}
