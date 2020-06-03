using System;

namespace AccuPay.Web.Account
{
    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }

        public static LoginException CredentialsMismatch() =>
            new LoginException(nameof(CredentialsMismatch));

        public static LoginException AccountDeactivated() =>
            new LoginException(nameof(AccountDeactivated));

        public static LoginException NoOrganization() =>
            new LoginException(nameof(NoOrganization));
    }
}
