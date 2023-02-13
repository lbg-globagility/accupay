using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("employee")]
    public class Employee : OrganizationalEntity
    {
        public const string EmployeeTypeDaily = "Daily";
        public const string EmployeeTypeMonthly = "Monthly";
        public const string EmployeeTypeFixed = "Fixed";

        public int? PositionID { get; set; }
        public int? PayFrequencyID { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

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
        public string Gender { get; set; }
        public string EmployeeType { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }

        //TODO: delete this
        public decimal LeaveBalance { get; set; }

        //TODO: delete this
        public decimal SickLeaveBalance { get; set; }

        [Column("LeaveAllowance")]
        public decimal VacationLeaveAllowance { get; set; }

        public decimal SickLeaveAllowance { get; set; }
        public decimal MaternityLeaveAllowance { get; set; }
        public decimal MaternityLeaveBalance { get; internal set; }
        public decimal OtherLeaveAllowance { get; set; }
        public decimal OtherLeaveBalance { get; internal set; }
        public bool AlphalistExempted { get; set; }

        [Obsolete("Moved to employment policy")]
        public decimal WorkDaysPerYear { get; set; }

        public int? DayOfRest { get; set; }
        public string AtmNo { get; set; }
        public string BankName { get; set; }

        [Obsolete("Moved to employment policy")]
        public bool CalcHoliday { get; set; }

        [Obsolete("Moved to employment policy")]
        public bool CalcSpecialHoliday { get; set; }

        [Obsolete("Moved to employment policy")]
        public bool CalcNightDiff { get; set; }

        [Obsolete("Moved to employment policy")]
        public bool CalcRestDay { get; set; }

        public DateTime? DateRegularized { get; set; }
        public DateTime? DateEvaluated { get; set; }
        public bool RevealInPayroll { get; set; }

        [Obsolete("Moved to employment policy")]
        public decimal LateGracePeriod { get; set; }

        public int? AgencyID { get; set; }
        public Byte[] Image { get; set; }
        public int AdvancementPoints { get; set; }
        public decimal BPIInsurance { get; set; }
        public int? BranchID { get; set; }
        public int? EmploymentPolicyId { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        [ForeignKey("BranchID")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("AgencyID")]
        public virtual Agency Agency { get; set; }

        [ForeignKey("PayFrequencyID")]
        public virtual PayFrequency PayFrequency { get; set; }

        public virtual EmploymentPolicy EmploymentPolicy { get; set; }

        public virtual ICollection<TimeEntry> TimeEntries { get; set; }

        public virtual ICollection<Paystub> Paystubs { get; set; }

        public virtual ICollection<ResetLeaveCreditItem> ResetLeaveCreditItems { get; set; }

        public int? OriginalImageId { get; set; }

        [ForeignKey("OriginalImageId")]
        public virtual File OriginalImage { get; set; }

        public bool GracePeriodAsBuffer { get; set; }

        public bool OvertimeOverride { get; set; }

        public string MiddleInitial
            => string.IsNullOrEmpty(MiddleName) ? null : MiddleName.Substring(0, 1);

        public bool IsDaily => EmployeeType?.ToLower() == EmployeeTypeDaily.ToLower();

        public bool IsMonthly => EmployeeType?.ToLower() == EmployeeTypeMonthly.ToLower();

        public bool IsFixed => EmployeeType?.ToLower() == EmployeeTypeFixed.ToLower();

        public bool IsWeeklyPaid => PayFrequencyID == (int)PayFrequencyType.Weekly;

        public bool IsPremiumInclusive => IsMonthly || IsFixed;

        public bool IsUnderAgency => AgencyID.HasValue;

        public string FullName => $"{FirstName} {LastName}".Trim();

        public string FullNameLastNameFirst => $"{LastName}, {FirstName}".Trim();

        public string FullNameWithMiddleInitialLastNameFirst =>
            $"{LastName}, {FirstName} {(MiddleInitial == null ? "" : MiddleInitial + ".")}".Trim();

        public string FullNameWithMiddleInitial =>
            $"{FirstName} {(MiddleInitial == null ? "" : MiddleInitial + ". ")}{LastName}".Trim();

        public string EmployeeIdWithPositionAndEmployeeType
            => $"ID# {EmployeeNo}, {Position?.Name}, {EmployeeType} Salary";

        public string SssSchedule => IsUnderAgency ?
            Position?.Division?.AgencySssDeductionSchedule :
            Position?.Division?.SssDeductionSchedule;

        public string PhilHealthSchedule => IsUnderAgency ?
            Position?.Division?.AgencyPhilHealthDeductionSchedule :
            Position?.Division?.PhilHealthDeductionSchedule;

        public string PagIBIGSchedule => IsUnderAgency ?
            Position?.Division?.AgencyPagIBIGDeductionSchedule :
            Position?.Division?.PagIBIGDeductionSchedule;

        public string WithholdingTaxSchedule => IsUnderAgency ?
            Position?.Division?.AgencyWithholdingTaxSchedule :
            Position?.Division?.WithholdingTaxSchedule;

        public bool IsActive => IsResigned == false && IsTerminated == false && IsRetired == false;

        public bool IsResigned => EmploymentStatus.Trim().ToUpper() == "RESIGNED";

        public bool IsTerminated => EmploymentStatus.Trim().ToUpper() == "TERMINATED";

        public bool IsRetired => EmploymentStatus.Trim().ToUpper() == "RETIRED";

        public bool IsWithinServicePeriod(DateTime currentDate) => IsActive ? IsActive :
            TerminationDate == null ? IsActive :
                currentDate <= TerminationDate.Value;

        public bool IsFirstPay(PayPeriod payPeriod)
        {
            return payPeriod != null &&
                payPeriod.PayFromDate.Date <= StartDate.Date &&
                StartDate.Date <= payPeriod.PayToDate.Date;
        }

        /// <summary>
        /// Checks if the employee is eligble for BPI Insurance and the employee was not given a BPI Insurance adjustment deduction yet this pay period.
        /// </summary>
        /// <param name="useBPIInsurancePolicy"></param>
        /// <param name="paystubAdjustments"></param>
        /// <param name="payPeriod"></param>
        /// <returns></returns>
        public bool IsEligibleForNewBPIInsurance(bool useBPIInsurancePolicy, ICollection<Adjustment> paystubAdjustments, PayPeriod payPeriod)
        {
            if (payPeriod == null || paystubAdjustments == null)
                return false;

            return useBPIInsurancePolicy &&
                IsFirstPay(payPeriod) &&
                BPIInsurance > 0 &&
                !paystubAdjustments.Any(a => a.Product?.PartNo == ProductConstant.BPI_INSURANCE_ADJUSTMENT);
        }

        public static Employee NewEmployee(int organizationId)
        {
            return new Employee
            {
                RowID = null,
                OrganizationID = organizationId,
                PayFrequencyID = PayrollTools.PayFrequencySemiMonthlyId,
                CalcHoliday = true,
                CalcNightDiff = true,
                CalcRestDay = true,
                CalcSpecialHoliday = true
            };
        }

        public static Employee NewEmployee(Organization organization)
        {
            return new Employee
            {
                RowID = null,
                OrganizationID = organization.RowID,
                PayFrequencyID = organization.IsWeekly ? PayrollTools.PayFrequencyWeeklyId : PayrollTools.PayFrequencySemiMonthlyId,
                CalcHoliday = true,
                CalcNightDiff = true,
                CalcRestDay = true,
                CalcSpecialHoliday = true
            };
        }

        public void SetOtherLeaveBalance(decimal otherLeaveBalance)
        {
            OtherLeaveBalance = otherLeaveBalance;
        }

        public void SetParentalLeaveBalance(decimal parentalLeaveBalance)
        {
            MaternityLeaveBalance = parentalLeaveBalance;
        }
    }
}
