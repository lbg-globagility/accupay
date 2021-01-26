using System.Collections.Generic;

namespace AccuPay.Core.Helpers
{
    public class UserConstants
    {
        private static Dictionary<UserStatus, string> indentifyUserStatus =
            new Dictionary<UserStatus, string>() {
                { UserStatus.Active, UserStatus.Active.ToString() },
                { UserStatus.Inactive, UserStatus.Inactive.ToString() }
            };

        public enum UserStatus
        {
            Active = 0,
            Inactive = 1
        }

        internal static string UserStatusToString(UserStatus userStatus)
        {
            return indentifyUserStatus[userStatus];
        }

        internal static string ACTIVE_STATUS
        {
            get
            {
                return UserStatus.Active.ToString();
            }
        }

        internal static string INACTIVE_STATUS
        {
            get
            {
                return UserStatus.Inactive.ToString();
            }
        }
    }
}