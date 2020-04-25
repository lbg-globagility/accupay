using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Helpers
{
    public class BreakTimeBracketHelper
    {
        public static decimal GetBreakTimeDuration(IList<BreakTimeBracket> breakTimeBrackets,
                                                    double shiftDuration)
        {
            breakTimeBrackets = breakTimeBrackets.
                                    OrderBy(b => b.ShiftDuration).
                                    ThenBy(b => b.BreakDuration).
                                    ToList();

            var lastBreakTimeBracket = new BreakTimeBracket();

            foreach (var breakTimeBracket in breakTimeBrackets)
            {
                lastBreakTimeBracket = breakTimeBracket;

                if (breakTimeBracket.ShiftDuration >= (decimal)shiftDuration)
                    break;
            }

            if (lastBreakTimeBracket == null)
                return 0;
            else
                return lastBreakTimeBracket.BreakDuration;
        }
    }
}