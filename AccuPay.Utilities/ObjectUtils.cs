using System;
using System.Globalization;

namespace AccuPay.Utilities
{
    public static class ObjectUtils
    {
        public static int ToInteger(object num)
        {
            int defaultOutput = 0;

            if (num == null) return defaultOutput;

            int output = defaultOutput;

            try
            {
                output = Convert.ToInt32(num);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static int? ToNullableInteger(object num)
        {
            int? defaultOutput = null;

            if (num == null) return defaultOutput;

            int? output = defaultOutput;

            try
            {
                output = Convert.ToInt32(num);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static decimal ToDecimal(object num, int? decimalPlace = null)
        {
            decimal defaultOutput = 0;

            if (num == null) return defaultOutput;

            decimal output = defaultOutput;

            try
            {
                output = Convert.ToDecimal(num);
                if (decimalPlace != null && decimalPlace > 0)
                {
                    AccuMath.CommercialRound(output, decimalPlace.Value);
                }
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static decimal? ToNullableDecimal(object num, int? decimalPlace = null)
        {
            decimal? defaultOutput = null;

            if (num == null) return defaultOutput;

            decimal? output = defaultOutput;

            try
            {
                output = Convert.ToDecimal(num);
                if (decimalPlace != null && decimalPlace > 0)
                {
                    AccuMath.CommercialRound(output, decimalPlace.Value);
                }
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static double? ToNullableDouble(object num, int? decimalPlace = null)
        {
            double? defaultOutput = null;

            if (num == null) return defaultOutput;

            double? output = defaultOutput;

            try
            {
                output = Convert.ToDouble(num);
                if (decimalPlace != null && decimalPlace > 0)
                {
                    AccuMath.CommercialRound(output.Value, decimalPlace.Value);
                }
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static DateTime ToDateTime(object dateInput)
        {
            DateTime defaultOutput = DateTime.MinValue;

            if (dateInput == null) return defaultOutput;

            DateTime output = defaultOutput;

            try
            {
                output = Convert.ToDateTime(dateInput);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static DateTime? ToNullableDateTime(object dateInput)
        {
            DateTime? defaultOutput = null;

            if (dateInput == null) return defaultOutput;

            DateTime? output = defaultOutput;

            try
            {
                output = Convert.ToDateTime(dateInput);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static bool ToBoolean(object booleanInput)
        {
            bool defaultOutput = false;

            if (booleanInput == null) return defaultOutput;

            bool output = defaultOutput;

            try
            {
                output = Convert.ToBoolean(booleanInput);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static bool? ToNullableBoolean(object booleanInput)
        {
            bool? defaultOutput = null;

            if (booleanInput == null) return defaultOutput;

            bool? output = defaultOutput;

            try
            {
                output = Convert.ToBoolean(booleanInput);
            }
            catch (Exception)
            {
                output = defaultOutput;
            }

            return output;
        }

        public static TimeSpan ToTimeSpan(object input)
        {
            TimeSpan defaultOutput = TimeSpan.Zero;

            if (input == null) return defaultOutput;

            TimeSpan output = defaultOutput;

            DateTime dt;

            if (DateTime.TryParseExact(input.ToString(), "HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                output = dt.TimeOfDay;
            }

            return output;
        }

        public static TimeSpan? ToNullableTimeSpan(object input)
        {
            TimeSpan? defaultOutput = null;

            if (input == null) return defaultOutput;

            TimeSpan? output = defaultOutput;

            DateTime dt;

            if (DateTime.TryParse(input.ToString(),
                CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                output = dt.TimeOfDay;
            }

            return output;
        }

        public static string ToStringOrNull(object text)
        {
            if (text == null) return null;
            return text.ToString();
        }

        public static string ToStringOrEmpty(object text)
        {
            if (text == null) return string.Empty;
            return text.ToString();
        }
    }
}