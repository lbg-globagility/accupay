using AccuPay.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Auth
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ICurrentUser _currentUser;

        public PermissionHandler(IRoleRepository roleRepository, ICurrentUser currentUser)
        {
            _roleRepository = roleRepository;
            _currentUser = currentUser;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var role = await _roleRepository.GetByUserAndOrganizationAsync(_currentUser.UserId, _currentUser.OrganizationId);

            if (role is null) return;

            if (role.IsAdmin)
            {
                context.Succeed(requirement);
            }

            if (role.HasPermission(requirement.Permission, requirement.Action))
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
