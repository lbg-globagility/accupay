using AccuPay.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employee")]
    public class Employee : IEmployee
    {
        [Key]
        public int? RowID { get; set; }

        public DateTime Created { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpd { get; set; }
        public int? LastUpdBy { get; set; }
        public int? OrganizationID { get; set; }
        public int? PositionID { get; set; }
        public int? PayFrequencyID { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Surname { get; set; }

        [Column("EmployeeID")]
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

        [Column("LeaveAllowance")]
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
        public Byte[] Image { get; set; }
        public int AdvancementPoints { get; set; }
        public decimal BPIInsurance { get; set; }
        public int? BranchID { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("PayFrequencyID")]
        public virtual PayFrequency PayFrequency { get; set; }

        //public virtual ICollection<Salary> Salaries { get; set; }

        public string MiddleInitial
        {
            get
            {
                return string.IsNullOrEmpty(MiddleName) ? null : MiddleName.Substring(0, 1);
            }
        }

        public bool IsDaily
        {
            get
            {
                return (EmployeeType.ToLower() == "daily"); // "Daily"
            }
        }

        public bool IsMonthly
        {
            get
            {
                return (EmployeeType.ToLower() == "monthly"); // "Monthly"
            }
        }

        public bool IsFixed
        {
            get
            {
                return (EmployeeType.ToLower() == "fixed"); // "Fixed"
            }
        }

        public bool IsWeeklyPaid
        {
            get
            {
                return PayFrequencyID == (int)PayFrequencyType.Weekly;
            }
        }

        public bool IsPremiumInclusive
        {
            get
            {
                return IsMonthly || IsFixed;
            }
        }

        public string FullNameLastNameFirst
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

        public string FullNameWithMiddleInitialLastNameFirst
        {
            get
            {
                return $"{LastName}, {FirstName} {MiddleInitial}";
            }
        }

        public string FullNameWithMiddleInitial
        {
            get
            {
                return $"{FirstName} {(MiddleInitial == null ? "" : MiddleInitial + ". ")}{LastName}";
            }
        }

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }

        public string EmployeeIdWithPositionAndEmployeeType
        {
            get
            {
                return $"ID# {EmployeeNo}, {Position?.Name}, {EmployeeType} Salary";
            }
        }

        public bool IsUnderAgency
        {
            get
            {
                return AgencyID.HasValue;
            }
        }

        public string SssSchedule
        {
            get
            {
                return IsUnderAgency ? Position?.Division?.AgencySssDeductionSchedule : Position?.Division?.SssDeductionSchedule;
            }
        }

        public string PhilHealthSchedule
        {
            get
            {
                return IsUnderAgency ? Position?.Division?.AgencyPhilHealthDeductionSchedule : Position?.Division?.PhilHealthDeductionSchedule;
            }
        }

        public string PagIBIGSchedule
        {
            get
            {
                return IsUnderAgency ? Position?.Division?.AgencyPagIBIGDeductionSchedule : Position?.Division?.PagIBIGDeductionSchedule;
            }
        }

        public string WithholdingTaxSchedule
        {
            get
            {
                return IsUnderAgency ? Position?.Division?.AgencyWithholdingTaxSchedule : Position?.Division?.WithholdingTaxSchedule;
            }
        }

        public bool IsActive
        {
            get
            {
                return IsResigned == false && IsTerminated == false && IsRetired == false;
            }
        }

        public bool IsResigned
        {
            get
            {
                return EmploymentStatus.Trim().ToUpper() == "RESIGNED";
            }
        }

        public bool IsTerminated
        {
            get
            {
                return EmploymentStatus.Trim().ToUpper() == "TERMINATED";
            }
        }

        public bool IsRetired
        {
            get
            {
                return EmploymentStatus.ToUpper().Trim() == "RETIRED";
            }
        }
    }
}