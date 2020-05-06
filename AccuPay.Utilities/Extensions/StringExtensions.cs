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

        /// <summary>
        /// Safely trims and convert the string to lower case even if it is null.
        /// Inside EF Core queries, best practice is to use this on the variable
        /// and not on the database column.
        /// </summary>
        /// <param name="text">The string to trim and convert to lower case.</param>
        /// <returns></returns>
        public static string ToTrimmedLowerCase(this string text)
        {
            // Calling this from EF core would not result to this
            // being translated into sql queries. Instead the database will
            // query all data then filter this in memory.

            // ALTHOUGH this will work to variables but not to database columns.
            // Ex: .Where(x => x.Trim()?.ToUpper() == str.ToTrimmedLowerCase()) is valid
            // Best practice is to use this on the variables since lambda does not allow a
            // null propagating operator.
            // Using Trim() and ToUpper() to null variable will crash the system
            return text?.Trim()?.ToLower();
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