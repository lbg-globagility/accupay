using System;

namespace AccuPay.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static TimeSpan? ToNullableTimeSpan(this string timespanString)
        {
            return ObjectUtils.ToNullableTimeSpan(timespanString);
        }

        public static decimal ToDecimal(this string num)
        {
            return ObjectUtils.ToDecimal(num);
        }

        public static decimal? ToNullableDecimal(this string num)
        {
            return ObjectUtils.ToNullableDecimal(num);
        }

        public static string Ellipsis(this string input, int maxCharacters)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if (input.Length > maxCharacters)
                {
                    string ellipsisString = "...";
                    return input.Substring(0, maxCharacters - ellipsisString.Length) + ellipsisString;
                }
            }

            return input;
        }
    }
}