using System;

namespace AccuPay.Data.Helpers
{
    public class UserConstants
    {
        public enum UserStatus
        {
            Active = 0,
            Inactive = 1
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