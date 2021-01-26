using Microsoft.AspNetCore.Authorization;

namespace AccuPay.Web.Core.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public readonly string Permission;

        public readonly string Action;

        public PermissionRequirement(string permission, string action)
        {
            Permission = permission;
            Action = action;
        }
    }
}
