using AccuPay.Data.Services.Policies;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public interface IPolicyHelper
    {
        bool AbsencesOnHoliday { get; }
        bool AllowanceForHoliday { get; }
        bool AllowanceForNightDiff { get; }
        bool AllowanceForNightDiffOT { get; }
        bool AllowanceForOvertime { get; }
        bool AllowanceForRestDay { get; }
        bool AllowanceForRestDayOT { get; }
        bool ComputeBreakTimeLate { get; }
        decimal DefaultBPIInsurance { get; }
        bool HasDifferentPayPeriodDates { get; }
        bool HasNightBreaktime { get; }
        string HolidayCalculationType { get; }
        bool IgnoreShiftOnRestDay { get; }
        bool LateHoursRoundingUp { get; }
        bool PaidAsLongAsHasTimeLog { get; }
        bool PostLegalHolidayCheck { get; }
        bool RequiredToWorkTheDayBeforeHoliday { get; }
        bool RespectDefaultRestDay { get; }
        bool RestDayInclusive { get; }
        ShiftBasedAutomaticOvertimePolicy ShiftBasedAutomaticOvertimePolicy { get; }
        bool ShowActual { get; }
        bool UseAgency { get; }
        bool UseBonus { get; }
        bool UseBPIInsurance { get; }
        bool UseCostCenter { get; }
        bool UseDefaultShiftAndTimeLogs { get; }
        bool UseEmailPayslip { get; }
        bool UseGoldwingsLoanInterest { get; }
        bool UseJobLevel { get; }
        bool UseLoanDeductFromBonus { get; }
        bool UseMassOvertime { get; }
        bool UseOffset { get; }
        bool UseUserLevel { get; }
        bool ValidateLeaveBalance { get; }

        DayValueSpan DefaultEndOfTheMonthDaysSpan(int? organizationId);
        DayValueSpan DefaultFirstHalfDaysSpan(int? organizationId);
        Task Refresh();
    }
}