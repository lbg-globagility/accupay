namespace AccuPay.Utilities.Extensions
{
    public static class DecimalExtensions
    {
        public static string RoundToString(this decimal input, int decimalPlace = 2)
        {
            string format = "#,##0.";

            for (int i = 1; i < decimalPlace; i++)
            {
                format += "0";
            }

            return AccuMath.CommercialRound(input, decimalPlace).ToString(format);
        }

        public static decimal Round(this decimal input, int decimalPlace = 2)
        {
            return AccuMath.CommercialRound(input, decimalPlace);
        }
    }
}