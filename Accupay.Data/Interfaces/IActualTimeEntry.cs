using System;

namespace AccuPay.Data
{
    public interface IActualTimeEntry
    {
        decimal AbsentDeduction { get; set; }
        decimal BasicDayPay { get; set; }
        DateTime Date { get; set; }
        int? EmployeeID { get; set; }
        int? EmployeeSalaryID { get; set; }
        int? EmployeeShiftID { get; set; }
        decimal HolidayPay { get; set; }
        decimal LateDeduction { get; set; }
        decimal LeavePay { get; set; }
        decimal NightDiffOTPay { get; set; }
        decimal NightDiffPay { get; set; }
        int? OrganizationID { get; set; }
        decimal OvertimePay { get; set; }
        int? PayRateID { get; set; }
        decimal RegularHolidayOTPay { get; set; }
        decimal RegularHolidayPay { get; set; }
        decimal RegularPay { get; set; }
        decimal RestDayOTPay { get; set; }
        decimal RestDayPay { get; set; }
        int? RowID { get; set; }
        decimal SpecialHolidayOTPay { get; set; }
        decimal SpecialHolidayPay { get; set; }
        decimal TotalDayPay { get; set; }
        decimal UndertimeDeduction { get; set; }
    }
}