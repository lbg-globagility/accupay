using AccuPay.Data.Enums;
using AccuPay.Data.Services.Policies;
using AccuPay.Utilities;

namespace AccuPay.Data.Services
{
    public class PolicyHelper
    {
        private readonly TimeEntryPolicy _policy;

        private readonly ListOfValueCollection _settings;

        public PolicyHelper(ListOfValueService listOfValueService)
        {
            _settings = listOfValueService.Create();

            _policy = new TimeEntryPolicy(_settings);
        }

        public bool ComputeBreakTimeLate => _policy.ComputeBreakTimeLate;

        public bool UseShiftSchedule => _policy.UseShiftSchedule;

        public bool RespectDefaultRestDay => _policy.RespectDefaultRestDay;

        public bool ValidateLeaveBalance => _policy.ValidateLeaveBalance;

        public PayRateCalculationBasis PayRateCalculationBasis =>
                _settings.GetEnum("Pay rate.CalculationBasis", PayRateCalculationBasis.Organization);

        public bool ShowActual => _settings.GetBoolean("Policy.ShowActual", true);

        public bool UseUserLevel => _settings.GetBoolean("User Policy.UseUserLevel", false);

        public bool UseEmailPayslip => _settings.GetBoolean("Payroll Policy.EmailPayslip", false);

        public decimal DefaultBPIInsurance => _settings.GetDecimal("Default.BPIInsurance");

        public bool ShowBranch => _settings.GetBoolean("Employee Policy.ShowBranch", false);

        public bool UseBPIInsurance => _settings.GetBoolean("Employee Policy.UseBPIInsurance", false);

        public bool UseDefaultShiftAndTimeLogs => _settings.GetBoolean("Data Policy.UseDefaultShiftAndTimeLogs", false);


        #region Pay Period Default Dates Policy ("16,31,false,true" means first day is "16", second days is "31", first day "is NOT last day of the month", second day "is last day of the month"

        public DaysSpan DefaultFirstHalfDaysSpan()
        {
            var value = _settings.GetString("Pay Period Policy.DefaultFirstHalfDaysSpan");

            return ParseDaysSpan(value, DaysSpan.DefaultFirstHalf);
        }

        public DaysSpan DefaultEndOfTheMonthDaysSpan()
        {
            var value = _settings.GetString("Pay Period Policy.DefaultEndOfTheMonthDaysSpan");

            return ParseDaysSpan(value, DaysSpan.DefaultEndOfTheMonth);
        }

        private DaysSpan ParseDaysSpan(string policyValue, DaysSpan defaultValue)
        {
            if (string.IsNullOrWhiteSpace(policyValue))
            {
                return defaultValue;
            }

            var values = policyValue.Split(',');
            if (values.Length < 2 || values.Length > 4)
            {
                return defaultValue;
            }

            int? startDay = ObjectUtils.ToNullableInteger(values[0]);
            int? endDay = ObjectUtils.ToNullableInteger(values[1]);

            if (startDay == null && endDay == null)
            {
                return defaultValue;
            }

            bool startDayIsLastDay = ObjectUtils.ToNullableBoolean(values[0]) ?? false;
            bool endDayIsLastDay = ObjectUtils.ToNullableBoolean(values[1]) ?? false;

            var startDayValue = DayValue.Create(startDay.Value, startDayIsLastDay);
            var endDayValue = DayValue.Create(endDay.Value, endDayIsLastDay);

            return DaysSpan.Create(startDayValue, endDayValue);
        }

        #endregion Pay Period Default Dates Policy ("16,31,false,true" means first day is "16", second days is "31", first day "is NOT last day of the month", second day "is last day of the month"
    }
}