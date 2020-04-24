using System.Collections.Generic;

namespace AccuPay.Data.Helpers
{
    public class ContributionSchedule
    {
        public const string FIRST_HALF = "First half";
        public const string END_OF_THE_MONTH = "End of the month";
        public const string PER_PAY_PERIOD = "Per pay period";

        public const string LAST_WEEK_OF_THE_MONTH = "Last week of the month";

        public static List<string> GetList()
        {
            return new List<string>()
        {
            FIRST_HALF,
            END_OF_THE_MONTH,
            PER_PAY_PERIOD
        };
        }
    }
}