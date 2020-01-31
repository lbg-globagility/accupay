using System;

namespace AccuPay.Utilities
{
    public static class AccuMath
    {
        /// <summary>
        /// Truncates and not round the fractional portion of the decimal to n places.
        /// </summary>
        /// <param name="value">The value to truncate</param>
        /// <param name="places">The number of fractional places to leave</param>
        /// <returns>The truncated decimal value</returns>
        public static decimal Truncate(decimal value, int places)
        {
            decimal x = (decimal)Math.Pow(10, places);
            return Math.Truncate(value * x) / x;
        }

        /// <summary>
        /// Perform a commercial rounding away from zero.
        ///
        /// ex:
        /// 1.284 -> 1.28
        /// 1.285 -> 1.29
        /// 1.286 -> 1.29
        /// </summary>
        /// <param name="value"></param>
        /// <param name="places"></param>
        /// <returns></returns>
        public static decimal CommercialRound(decimal value, int places = 2)
        {
            return Math.Round(value, places, MidpointRounding.AwayFromZero);
        }

        public static decimal CommercialRound(decimal? value, int places = 2)
        {
            if (value == null) return 0M;

            return CommercialRound(value.Value, places);
        }

        public static decimal CommercialRound(double value, int places = 2)
        {
            return CommercialRound((decimal)value, places);
        }

        /// <summary>
        /// This method is used as a workaround on VB.Net's fuck up ternary operator using nullable decimal. Example of built-in ternary operator: If(value, 0, Nothing). If it's true, it return 0. If it's false, it should return Nothing but VB.Net is a fuck up PL and returns zero instead.This is due to VB.Net's "Nothing" actually a closer match to C#'s default(T) instead of null. And the value of default(Integer) is 0.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="trueOutput">The output when the condition is True.</param>
        /// <param name="falseOutput">The output when the condition is False.</param>
        /// <returns>A nullable decimal.</returns>
        public static decimal? NullableDecimalTernaryOperator(
                                bool condition,
                                decimal? trueOutput,
                                decimal? falseOutput)
        {
            return condition ? trueOutput : falseOutput;
        }
    }
}