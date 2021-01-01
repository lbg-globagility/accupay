namespace AccuPay.Core.Services
{
    public class AllowancePolicy
    {
        private readonly ListOfValueCollection _settings;

        public AllowancePolicy(ListOfValueCollection settings) => _settings = settings;

        public bool IsLeavePaid => _settings.GetBoolean("AllowancePolicy.IsLeavePaid");

        public bool IsOvertimePaid => _settings.GetBoolean("AllowancePolicy.IsOvertimePaid");

        public bool IsRestDayPaid => _settings.GetBoolean("AllowancePolicy.IsRestDayPaid");

        public bool IsRestDayOTPaid => _settings.GetBoolean("AllowancePolicy.IsRestDayOTPaid");

        public bool IsSpecialHolidayPaid => _settings.GetBoolean("AllowancePolicy.IsSpecialHolidayPaid");

        public bool IsRegularHolidayPaid => _settings.GetBoolean("AllowancePolicy.IsRegularHolidayPaid");
    }
}