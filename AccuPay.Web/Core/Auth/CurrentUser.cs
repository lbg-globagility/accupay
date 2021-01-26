using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AccuPay.Web.Core.Auth
{
    public class CurrentUser : ICurrentUser
    {
        public int UserId { get; private set; }

        public int OrganizationId { get; private set; }

        public int ClientId { get; private set; }

        public int? EmployeeId { get; private set; }

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
            ReadEmployeeId(user);
        }

        private void ReadUserId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            UserId = value is null ? 0 : int.Parse(value);
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

        private void ReadEmployeeId(ClaimsPrincipal principal)
        {
            var value = principal.FindFirst(CustomClaimTypes.EmployeeId)?.Value;

            EmployeeId = string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        }
    }
}
