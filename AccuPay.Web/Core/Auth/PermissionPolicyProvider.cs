using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Auth
{
    public class PermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private const string PolicyPrefix = "Permission";

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() =>
            Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() =>
            Task.FromResult<AuthorizationPolicy>(null);

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PolicyPrefix))
            {
                var permission = policyName.Substring(PolicyPrefix.Length);

                var words = permission.Split(":");
                if (words.Length != 2)
                {
                    return Task.FromResult<AuthorizationPolicy>(null);
                }

                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new PermissionRequirement(words[0], words[1]));

                return Task.FromResult(policy.Build());
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }
    }
}
