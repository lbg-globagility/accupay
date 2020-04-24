using System;

namespace AccuPay.Data
{
    public interface ITimeEntry
    {
        decimal AbsentDeduction { get; set; }
        decimal AbsentHours { get; set; }
        decimal BasicDayPay { get; set; }
        decimal BasicHours { get; set; }
        decimal BasicRegularHolidayPay { get; set; }
        int? BranchID { get; set; }
        DateTime Date { get; set; }
        DateTime DutyEnd { get; set; }
        DateTime DutyStart { get; set; }

        //IEmployee Employee { get; set; }
        int? EmployeeID { get; set; }

        int? EmployeeSalaryID { get; set; }
        int? EmployeeShiftID { get; set; }
        bool HasShift { get; set; }
        decimal HolidayPay { get; set; }
        bool IsRestDay { get; set; }
        decimal LateDeduction { get; set; }
        decimal LateHours { get; set; }
        decimal LeavePay { get; set; }
        decimal MaternityLeaveHours { get; set; }
        decimal NightDiffHours { get; set; }
        decimal NightDiffOTHours { get; set; }
        decimal NightDiffOTPay { get; set; }
        decimal NightDiffPay { get; set; }
        int? OrganizationID { get; set; }
        decimal OtherLeaveHours { get; set; }
        decimal OvertimeHours { get; set; }
        decimal OvertimePay { get; set; }
        int? PayRateID { get; set; }
        decimal RegularHolidayHours { get; set; }
        decimal RegularHolidayOTHours { get; set; }
        decimal RegularHolidayOTPay { get; set; }
        decimal RegularHolidayPay { get; set; }
        decimal RegularHours { get; set; }
        decimal RegularPay { get; set; }
        decimal RestDayHours { get; set; }
        decimal RestDayOTHours { get; set; }
        decimal RestDayOTPay { get; set; }
        decimal RestDayPay { get; set; }
        int? RowID { get; set; }
        decimal ShiftHours { get; set; }
        decimal SickLeaveHours { get; set; }
        decimal SpecialHolidayHours { get; set; }
        decimal SpecialHolidayOTHours { get; set; }
        decimal SpecialHolidayOTPay { get; set; }
        decimal SpecialHolidayPay { get; set; }
        decimal TotalDayPay { get; set; }
        decimal TotalHours { get; set; }
        decimal TotalLeaveHours { get; }
        decimal UndertimeDeduction { get; set; }
        decimal UndertimeHours { get; set; }
        decimal VacationLeaveHours { get; set; }
        decimal WorkHours { get; set; }

        void ComputeTotalHours();

        void ComputeTotalPay();

        decimal GetTotalDayPay();

        void Reset();

        void SetLeaveHours(string type, decimal leaveHours);
    }
}