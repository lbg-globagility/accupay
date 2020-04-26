using System;
using System.Globalization;
using System.Text.RegularExpressions;

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

        public static string ToTrimmedLowerCase(this string text)
        {
            // Calling this from EF core would not result to this
            // being translated into sql queries. Instead the database will
            // query all data then filter this in memory.

            // Although this will work to variables but not to database columns.
            // Ex: .Where(x => x.Trim()?.ToUpper() == str.ToTrimmedLowerCase()) is valid
            return text?.Trim()?.ToUpper();
        }

        public static string ToPascal(this string text)
        {
            if (text == null) return null;

            string newText = Regex.Replace(text, "([A-Z])", " $1");

            var info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(newText).Replace(" ", string.Empty);
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