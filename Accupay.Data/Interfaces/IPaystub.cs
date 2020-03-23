using System;

namespace AccuPay.Data
{
    public interface IPaystub
    {
        decimal AbsenceDeduction { get; set; }
        decimal AbsentHours { get; set; }
        decimal BasicHours { get; set; }
        decimal BasicPay { get; set; }
        DateTime Created { get; set; }
        int? CreatedBy { get; set; }
        decimal DeferredTaxableIncome { get; set; }
        int? EmployeeID { get; set; }
        bool FirstTimeSalary { get; set; }
        decimal GrossPay { get; set; }
        decimal HdmfEmployeeShare { get; set; }
        decimal HdmfEmployerShare { get; set; }
        decimal HolidayPay { get; set; }
        DateTime? LastUpd { get; set; }
        int? LastUpdBy { get; set; }
        decimal LateDeduction { get; set; }
        decimal LateHours { get; set; }
        decimal LeaveHours { get; set; }
        decimal LeavePay { get; set; }
        decimal NetPay { get; set; }
        decimal NightDiffHours { get; set; }
        decimal NightDiffOvertimeHours { get; set; }
        decimal NightDiffOvertimePay { get; set; }
        decimal NightDiffPay { get; set; }
        int? OrganizationID { get; set; }
        decimal OvertimeHours { get; set; }
        decimal OvertimePay { get; set; }
        DateTime PayFromdate { get; set; }
        int? PayPeriodID { get; set; }
        DateTime PayToDate { get; set; }
        decimal PhilHealthEmployeeShare { get; set; }
        decimal PhilHealthEmployerShare { get; set; }
        decimal RegularHolidayHours { get; set; }
        decimal RegularHolidayOTHours { get; set; }
        decimal RegularHolidayOTPay { get; set; }
        decimal RegularHolidayPay { get; set; }
        decimal RegularHolidayRestDayPay { get; set; }
        decimal RegularHours { get; set; }
        decimal RegularPay { get; set; }
        decimal RestDayHours { get; set; }
        decimal RestDayOTHours { get; set; }
        decimal RestDayOTPay { get; set; }
        decimal RestDayPay { get; set; }
        int? RowID { get; set; }
        decimal SpecialHolidayHours { get; set; }
        decimal SpecialHolidayOTHours { get; set; }
        decimal SpecialHolidayOTPay { get; set; }
        decimal SpecialHolidayPay { get; set; }
        decimal SpecialHolidayRestDayPay { get; set; }
        decimal SssEmployeeShare { get; set; }
        decimal SssEmployerShare { get; set; }
        decimal TaxableIncome { get; set; }
        bool ThirteenthMonthInclusion { get; set; }
        int? TimeEntryID { get; set; }
        decimal TotalAdjustments { get; set; }
        decimal TotalAllowance { get; set; }
        decimal TotalBonus { get; set; }
        decimal TotalEarnings { get; set; }
        decimal TotalLoans { get; set; }
        decimal TotalTaxableAllowance { get; set; }
        decimal TotalUndeclaredSalary { get; set; }
        decimal TotalVacationDaysLeft { get; set; }
        decimal UndertimeDeduction { get; set; }
        decimal UndertimeHours { get; set; }
        decimal WithholdingTax { get; set; }
    }
}