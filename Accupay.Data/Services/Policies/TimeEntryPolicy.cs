namespace AccuPay.Data.Services
{
    public class TimeEntryPolicy
    {
        private readonly ListOfValueCollection _settings;

        public TimeEntryPolicy(ListOfValueCollection settings) => _settings = settings;

        public bool LateHoursRoundingUp =>
                        _settings.GetBoolean("LateHours.RoundingUp");

        public bool NightDiffBreakTime =>
                        _settings.GetBoolean("NightDiffPolicy.HasBreakTime");

        public bool LateSkipCountRounding =>
                        _settings.GetBoolean("LateHours.SkipCountRounding");

        public bool ComputeBreakTimeLate =>
                        _settings.GetBoolean("LateHours.ComputeBreakTimeLate");

        public bool OvertimeSkipCountRounding =>
                        _settings.GetBoolean("OvertimeHours.SkipCountRounding");

        public bool AbsencesOnHoliday =>
                        _settings.GetBoolean("Payroll Policy.holiday.allowabsence");

        public bool RequiredToWorkLastDay =>
                        _settings.GetBoolean("Payroll Policy.HolidayLastWorkingDayOrAbsent");

        public bool RequiredToWorkLastDayForHolidayPay =>
                        _settings.GetBoolean("HolidayPolicy.WorkLastDayForHolidayPay");

        public bool HasNightBreaktime =>
                        _settings.GetBoolean("OvertimePolicy.NightBreaktime");

        public bool RespectDefaultRestDay =>
                        _settings.GetBoolean("RestDayPolicy.RespectDefaultRestDay");

        public bool IgnoreShiftOnRestDay =>
                        _settings.GetBoolean("RestDayPolicy.IgnoreShiftOnRestDay");

        public bool RestDayInclusive =>
                        _settings.GetBoolean("Payroll Policy.restday.inclusiveofbasicpay");

        public bool UseShiftSchedule =>
                        _settings.GetBoolean("ShiftPolicy.UseShiftSchedule");

        public bool ValidateLeaveBalance =>
                        _settings.GetBoolean("LeaveBalancePolicy.Validate");

        public bool PaidAsLongAsPresent =>
                        _settings.GetBoolean("TimeEntry.TimeLogAsPaymentRequirement");

        public bool PostLegalHolidayCheck =>
                        _settings.GetBoolean("HolidayPolicy.PostLegalHolidayCheck");
    }
}