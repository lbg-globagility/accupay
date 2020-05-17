using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Employees.Models
{
    public class EmployeeDto
    {
        public static EmployeeDto Produce(Employee employee)
        {
            return FromEmployeeToDto(employee);
        }

        internal static EmployeeDto FromEmployeeToDto(Employee _employee)
        {
            return new EmployeeDto()
            {
                RowID = _employee.RowID,
                PositionID = _employee.PositionID,
                PayFrequencyID = _employee.PayFrequencyID,
                Salutation = _employee.Salutation,
                FirstName = _employee.FirstName,
                MiddleName = _employee.MiddleName,
                LastName = _employee.LastName,
                Surname = _employee.Surname,
                EmployeeNo = _employee.EmployeeNo,
                TinNo = _employee.TinNo,
                SssNo = _employee.SssNo,
                HdmfNo = _employee.HdmfNo,
                PhilHealthNo = _employee.PhilHealthNo,
                EmploymentStatus = _employee.EmploymentStatus,
                EmailAddress = _employee.EmailAddress,
                WorkPhone = _employee.WorkPhone,
                HomePhone = _employee.HomePhone,
                MobilePhone = _employee.MobilePhone,
                HomeAddress = _employee.HomeAddress,
                Nickname = _employee.Nickname,
                JobTitle = _employee.JobTitle,
                Gender = _employee.Gender,
                EmployeeType = _employee.EmployeeType,
                MaritalStatus = _employee.MaritalStatus,
                BirthDate = _employee.BirthDate,
                StartDate = _employee.StartDate,
                TerminationDate = _employee.TerminationDate,
                NoOfDependents = _employee.NoOfDependents,
                UndertimeOverride = _employee.UndertimeOverride,
                OvertimeOverride = _employee.OvertimeOverride,
                NewEmployeeFlag = _employee.NewEmployeeFlag,
                LeaveBalance = _employee.LeaveBalance,
                SickLeaveBalance = _employee.SickLeaveBalance,
                MaternityLeaveBalance = _employee.MaternityLeaveBalance,
                OtherLeaveBalance = _employee.OtherLeaveBalance,
                VacationLeaveAllowance = _employee.VacationLeaveAllowance,
                SickLeaveAllowance = _employee.SickLeaveAllowance,
                MaternityLeaveAllowance = _employee.MaternityLeaveAllowance,
                OtherLeaveAllowance = _employee.OtherLeaveAllowance,
                LeavePerPayPeriod = _employee.LeavePerPayPeriod,
                SickLeavePerPayPeriod = _employee.SickLeavePerPayPeriod,
                MaternityLeavePerPayPeriod = _employee.MaternityLeavePerPayPeriod,
                OtherLeavePerPayPeriod = _employee.OtherLeavePerPayPeriod,
                AlphalistExempted = _employee.AlphalistExempted,
                WorkDaysPerYear = _employee.WorkDaysPerYear,
                DayOfRest = _employee.DayOfRest,
                AtmNo = _employee.AtmNo,
                BankName = _employee.BankName,
                CalcHoliday = _employee.CalcHoliday,
                CalcSpecialHoliday = _employee.CalcSpecialHoliday,
                CalcNightDiff = _employee.CalcNightDiff,
                CalcNightDiffOT = _employee.CalcNightDiffOT,
                CalcRestDay = _employee.CalcRestDay,
                CalcRestDayOT = _employee.CalcRestDayOT,
                DateRegularized = _employee.DateRegularized,
                DateEvaluated = _employee.DateEvaluated,
                RevealInPayroll = _employee.RevealInPayroll,
                LateGracePeriod = _employee.LateGracePeriod,
                OffsetBalance = _employee.OffsetBalance,
                AgencyID = _employee.AgencyID,
                AdvancementPoints = _employee.AdvancementPoints,
                BPIInsurance = _employee.BPIInsurance,
            };
        }

        public int? RowID { get; set; }
        public int? PositionID { get; set; }
        public int? PayFrequencyID { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string EmployeeNo { get; set; }
        public string TinNo { get; set; }
        public string SssNo { get; set; }
        public string HdmfNo { get; set; }
        public string PhilHealthNo { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string HomeAddress { get; set; }
        public string Nickname { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }
        public string EmployeeType { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? NoOfDependents { get; set; }
        public bool UndertimeOverride { get; set; }
        public bool OvertimeOverride { get; set; }
        public bool NewEmployeeFlag { get; set; }
        public decimal LeaveBalance { get; set; }
        public decimal SickLeaveBalance { get; set; }
        public decimal MaternityLeaveBalance { get; set; }
        public decimal OtherLeaveBalance { get; set; }
        public decimal VacationLeaveAllowance { get; set; }
        public decimal SickLeaveAllowance { get; set; }
        public decimal MaternityLeaveAllowance { get; set; }
        public decimal OtherLeaveAllowance { get; set; }
        public decimal LeavePerPayPeriod { get; set; }
        public decimal SickLeavePerPayPeriod { get; set; }
        public decimal MaternityLeavePerPayPeriod { get; set; }
        public decimal OtherLeavePerPayPeriod { get; set; }
        public bool AlphalistExempted { get; set; }
        public decimal WorkDaysPerYear { get; set; }
        public int? DayOfRest { get; set; }
        public string AtmNo { get; set; }
        public string BankName { get; set; }
        public bool CalcHoliday { get; set; }
        public bool CalcSpecialHoliday { get; set; }
        public bool CalcNightDiff { get; set; }
        public bool CalcNightDiffOT { get; set; }
        public bool CalcRestDay { get; set; }
        public bool CalcRestDayOT { get; set; }
        public DateTime? DateRegularized { get; set; }
        public DateTime? DateEvaluated { get; set; }
        public bool RevealInPayroll { get; set; }
        public decimal LateGracePeriod { get; set; }
        public decimal OffsetBalance { get; set; }
        public int? AgencyID { get; set; }
        public int AdvancementPoints { get; set; }
        public decimal BPIInsurance { get; set; }
    }
}
