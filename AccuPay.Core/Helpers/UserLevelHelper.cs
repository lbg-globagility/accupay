using AccuPay.Core.Enums;

namespace AccuPay.Core.Helpers
{
    public class UserLevelHelper
    {
        public static string GetUserLevelDescription(UserLevel userLevel)
        {
            switch ((userLevel))
            {
                case UserLevel.One:
                    return "One";

                case UserLevel.Two:
                    return "Two";

                case UserLevel.Three:
                    return "Three";

                case UserLevel.Four:
                    return "Four";

                case UserLevel.Five:
                    return "Five";
            }

            return "";
        }
    }
}