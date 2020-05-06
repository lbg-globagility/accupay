namespace AccuPay.Data.Services
{
    public class ActualTimeEntryPolicy
    {
        private readonly ListOfValueCollection _settings;

        public ActualTimeEntryPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool AllowanceForOvertime =>
            _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForOvertime", true);

        public bool AllowanceForNightDiff =>
        _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForNightDifferential", true);

        public bool AllowanceForNightDiffOT =>
            _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForNightDifferentialOvertime", true);

        public bool AllowanceForRestDay =>
            _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForRestDay", true);

        public bool AllowanceForRestDayOT =>
            _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForRestDayOT", true);

        public bool AllowanceForHoliday =>
            _settings.GetBoolean("Payroll Policy.PaySalaryAllowanceForHolidayPay", true);
    }
}