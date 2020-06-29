using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Employees.Models
{
    public class EmployeeDto : BaseEmployeeDto
    {
        public static EmployeeDto Convert(Employee employee)
        {
            var dto = new EmployeeDto();
            if (employee == null) return dto;

            dto.ApplyData(employee);

            return dto;
        }

        protected override void ApplyData(Employee employee)
        {
            if (employee == null) return;

            base.ApplyData(employee);

            Id = employee.RowID;
            PositionID = employee.PositionID;
            PayFrequencyID = employee.PayFrequencyID;
            Salutation = employee.Salutation;
            FirstName = employee.FirstName;
            MiddleName = employee.MiddleName;
            LastName = employee.LastName;
            Surname = employee.Surname;
            Tin = employee.TinNo;
            SssNo = employee.SssNo;
            PhilHealthNo = employee.PhilHealthNo;
            PagIbigNo = employee.HdmfNo;
            EmploymentStatus = employee.EmploymentStatus;
            EmailAddress = employee.EmailAddress;
            WorkPhone = employee.WorkPhone;
            LandlineNo = employee.HomePhone;
            MobileNo = employee.MobilePhone;
            Address = employee.HomeAddress;
            Nickname = employee.Nickname;
            JobTitle = employee.JobTitle;
            Gender = employee.Gender;
            EmployeeType = employee.EmployeeType;
            MaritalStatus = employee.MaritalStatus;
            Birthdate = employee.BirthDate;
            StartDate = employee.StartDate;
            TerminationDate = employee.TerminationDate;
            NoOfDependents = employee.NoOfDependents;
            UndertimeOverride = employee.UndertimeOverride;
            OvertimeOverride = employee.OvertimeOverride;
            NewEmployeeFlag = employee.NewEmployeeFlag;
            LeaveBalance = employee.LeaveBalance;
            SickLeaveBalance = employee.SickLeaveBalance;
            MaternityLeaveBalance = employee.MaternityLeaveBalance;
            OtherLeaveBalance = employee.OtherLeaveBalance;
            VacationLeaveAllowance = employee.VacationLeaveAllowance;
            SickLeaveAllowance = employee.SickLeaveAllowance;
            MaternityLeaveAllowance = employee.MaternityLeaveAllowance;
            OtherLeaveAllowance = employee.OtherLeaveAllowance;
            LeavePerPayPeriod = employee.LeavePerPayPeriod;
            SickLeavePerPayPeriod = employee.SickLeavePerPayPeriod;
            MaternityLeavePerPayPeriod = employee.MaternityLeavePerPayPeriod;
            OtherLeavePerPayPeriod = employee.OtherLeavePerPayPeriod;
            AlphalistExempted = employee.AlphalistExempted;
            WorkDaysPerYear = employee.WorkDaysPerYear;
            DayOfRest = employee.DayOfRest;
            AtmNo = employee.AtmNo;
            BankName = employee.BankName;
            CalcHoliday = employee.CalcHoliday;
            CalcSpecialHoliday = employee.CalcSpecialHoliday;
            CalcNightDiff = employee.CalcNightDiff;
            CalcRestDay = employee.CalcRestDay;
            DateRegularized = employee.DateRegularized;
            DateEvaluated = employee.DateEvaluated;
            RevealInPayroll = employee.RevealInPayroll;
            LateGracePeriod = employee.LateGracePeriod;
            OffsetBalance = employee.OffsetBalance;
            AgencyID = employee.AgencyID;
            AdvancementPoints = employee.AdvancementPoints;
            BPIInsurance = employee.BPIInsurance;
            FullName = employee.FullNameWithMiddleInitialLastNameFirst;
        }

        public int? Id { get; set; }
        public int? PositionID { get; set; }
        public int? PayFrequencyID { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }
        public string Tin { get; set; }
        public string SssNo { get; set; }
        public string PagIbigNo { get; set; }
        public string PhilHealthNo { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public string LandlineNo { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Nickname { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }
        public string EmployeeType { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime Birthdate { get; set; }
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
        public string FullName { get; set; }
    }
}
