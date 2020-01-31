using System.Globalization;
using System.Text.RegularExpressions;

namespace AccuPay.Utilities
{
    public static class StringUtils
    {
        public static string ToPascal(string text)
        {
            if (text == null) return null;

            string newText = Regex.Replace(text, "([A-Z])", " $1");

            var info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(newText).Replace(" ", string.Empty);
        }

        public static string Normalize(string text)
        {
            return text?.Trim()?.ToUpper();
        }
    }
}