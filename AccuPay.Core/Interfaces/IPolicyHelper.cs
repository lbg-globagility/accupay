using AccuPay.Core.Enums;
using AccuPay.Core.Services.Policies;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
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
        bool IsPolicyByOrganization { get; }
        bool LateHoursRoundingUp { get; }
        bool PaidAsLongAsHasTimeLog { get; }
        bool PostLegalHolidayCheck { get; }
        bool RequiredToWorkTheDayBeforeHoliday { get; }
        bool RespectDefaultRestDay { get; }
        bool RestDayInclusive { get; }
        ShiftBasedAutomaticOvertimePolicy ShiftBasedAutomaticOvertimePolicy { get; }
        bool ShowActual { get; }

        SssCalculationBasis SssCalculationBasis(int organizationId);

        bool UseAgency { get; }
        bool UseBonus { get; }
        bool UseBPIInsurance { get; }
        bool UseGracePeriodAsBuffer { get; }

        bool UseCostCenter { get; }
        bool UseDefaultShiftAndTimeLogs { get; }
        bool UseEmailPayslip { get; }
        bool UseGoldwingsLoanInterest { get; }
        decimal GoldWingsLoanInterestDefault { get; }
        bool UseJobLevel { get; }
        bool UseLoanDeductFromBonus { get; }
        bool UseLoanDeductFromThirteenthMonthPay { get; }
        bool UseMassOvertime { get; }
        bool UseOffset { get; }
        bool UseUserLevel { get; }
        bool ValidateLeaveBalance { get; }
        bool OverrideOvertimeRateEligibility { get; }
        bool IsMultipleGracePeriod { get; }
        ILeavePolicy GetLeavePolicy { get; }
        ILeaveResetPolicy GetLeaveResetPolicy { get; }
        bool IsEnableCashoutUnusedLeaves { get; }

        DayValueSpan DefaultEndOfTheMonthDaysSpan(int? organizationId);

        DayValueSpan DefaultFirstHalfDaysSpan(int? organizationId);

        Task Refresh();

        SssPolicy SssPolicy { get; }
        HdmfPolicy HdmfPolicy { get; }
    }
}
