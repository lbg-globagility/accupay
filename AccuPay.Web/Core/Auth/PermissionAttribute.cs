using Microsoft.AspNetCore.Authorization;

namespace AccuPay.Web.Core.Auth
{
    /// <summary>
    /// Custom code attribute for permissions handling on the server side.
    ///
    /// <code>
    /// [Permission(PermissionTypes.SecretMessage)]
    /// public void SecretMessage()
    /// {
    ///     return "Secret handshake";
    /// }
    /// </code>
    /// </summary>
    public class PermissionAttribute : AuthorizeAttribute
    {
        private const string PolicyPrefix = "Permission";

        public PermissionAttribute(string permissionName)
        {
            Policy = $"{PolicyPrefix}{permissionName}";
        }
    }
}
