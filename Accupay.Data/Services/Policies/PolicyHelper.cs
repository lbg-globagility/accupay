using AccuPay.Data.Enums;

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
    }
}