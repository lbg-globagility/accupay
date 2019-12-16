using System;

namespace Accupay.Data
{
    public interface IEmployee
    {
        int AdvancementPoints { get; set; }
        int? AgencyID { get; set; }
        bool AlphalistExempted { get; set; }
        string AtmNo { get; set; }
        string BankName { get; set; }
        DateTime BirthDate { get; set; }
        bool CalcHoliday { get; set; }
        bool CalcNightDiff { get; set; }
        bool CalcNightDiffOT { get; set; }
        bool CalcRestDay { get; set; }
        bool CalcRestDayOT { get; set; }
        bool CalcSpecialHoliday { get; set; }
        DateTime Created { get; set; }
        int? CreatedBy { get; set; }
        DateTime? DateEvaluated { get; set; }
        DateTime? DateRegularized { get; set; }
        int? DayOfRest { get; set; }
        string EmailAddress { get; set; }
        string EmployeeNo { get; set; }
        string EmployeeType { get; set; }
        string EmploymentStatus { get; set; }
        string FirstName { get; set; }
        string Gender { get; set; }
        string HdmfNo { get; set; }
        string HomeAddress { get; set; }
        string HomePhone { get; set; }
        byte[] Image { get; set; }
        string JobTitle { get; set; }
        string LastName { get; set; }
        DateTime? LastUpd { get; set; }
        int? LastUpdBy { get; set; }
        decimal LateGracePeriod { get; set; }
        decimal LeaveAllowance { get; set; }
        decimal LeaveBalance { get; set; }
        decimal LeavePerPayPeriod { get; set; }
        string MaritalStatus { get; set; }
        decimal MaternityLeaveAllowance { get; set; }
        decimal MaternityLeaveBalance { get; set; }
        decimal MaternityLeavePerPayPeriod { get; set; }
        string MiddleName { get; set; }
        string MobilePhone { get; set; }
        bool NewEmployeeFlag { get; set; }
        string Nickname { get; set; }
        int? NoOfDependents { get; set; }
        decimal OffsetBalance { get; set; }
        int? OrganizationID { get; set; }
        decimal OtherLeaveAllowance { get; set; }
        decimal OtherLeaveBalance { get; set; }
        decimal OtherLeavePerPayPeriod { get; set; }
        bool OvertimeOverride { get; set; }
        int? PayFrequencyID { get; set; }
        string PhilHealthNo { get; set; }
        int? PositionID { get; set; }
        bool RevealInPayroll { get; set; }
        int? RowID { get; set; }
        string Salutation { get; set; }
        decimal SickLeaveAllowance { get; set; }
        decimal SickLeaveBalance { get; set; }
        decimal SickLeavePerPayPeriod { get; set; }
        string SssNo { get; set; }
        DateTime StartDate { get; set; }
        string Surname { get; set; }
        DateTime? TerminationDate { get; set; }
        string TinNo { get; set; }
        bool UndertimeOverride { get; set; }
        decimal WorkDaysPerYear { get; set; }
        string WorkPhone { get; set; }
    }
}