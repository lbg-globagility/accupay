using AccuPay.Core.Services.Policies;

namespace AccuPay.Core.Services
{
    public class TimeEntryPolicy
    {
        private readonly ListOfValueCollection _settings;

        public TimeEntryPolicy(ListOfValueCollection settings)
        {
            _settings = settings;

            ShiftBasedAutomaticOvertimePolicy = new ShiftBasedAutomaticOvertimePolicy(_settings);
        }

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

        public bool RequiredToWorkTheDayBeforeHoliday =>
                        _settings.GetBoolean("Payroll Policy.HolidayLastWorkingDayOrAbsent");

        public bool HasNightBreaktime =>
                        _settings.GetBoolean("OvertimePolicy.NightBreaktime");

        public bool RespectDefaultRestDay =>
                        _settings.GetBoolean("RestDayPolicy.RespectDefaultRestDay");

        public bool IgnoreShiftOnRestDay =>
                        _settings.GetBoolean("RestDayPolicy.IgnoreShiftOnRestDay");

        public bool RestDayInclusive =>
                        _settings.GetBoolean("Payroll Policy.restday.inclusiveofbasicpay");

        public bool ValidateLeaveBalance =>
                        _settings.GetBoolean("LeaveBalancePolicy.Validate");

        public bool PaidAsLongAsHasTimeLog =>
                        _settings.GetBoolean("TimeEntry Policy.PaidAsLongAsHasTimeLog");

        public bool PostLegalHolidayCheck =>
                        _settings.GetBoolean("HolidayPolicy.PostLegalHolidayCheck");

        public ShiftBasedAutomaticOvertimePolicy ShiftBasedAutomaticOvertimePolicy { get; }
    }
}