using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("paystub")]
    public class Paystub : BasePaystub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public decimal BasicHours { get; set; }

        public decimal RegularHours { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffOvertimeHours { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal LeaveHours { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal LateHours { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal RestDayNightDiffHours { get; set; }
        public decimal RestDayNightDiffOTHours { get; set; }

        public decimal SpecialHolidayNightDiffHours { get; set; }
        public decimal SpecialHolidayNightDiffOTHours { get; set; }
        public decimal SpecialHolidayRestDayHours { get; set; }
        public decimal SpecialHolidayRestDayOTHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffHours { get; set; }
        public decimal SpecialHolidayRestDayNightDiffOTHours { get; set; }

        public decimal RegularHolidayNightDiffHours { get; set; }
        public decimal RegularHolidayNightDiffOTHours { get; set; }
        public decimal RegularHolidayRestDayHours { get; set; }
        public decimal RegularHolidayRestDayOTHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffHours { get; set; }
        public decimal RegularHolidayRestDayNightDiffOTHours { get; set; }

        [Column("WorkPay")]
        public decimal TotalEarnings { get; set; }

        public decimal TotalBonus { get; set; }

        [Column("TotalAllowance")]
        public decimal TotalNonTaxableAllowance { get; set; }

        public decimal TotalTaxableAllowance { get; set; }

        [Column("DeferredTaxableIncome")]
        public decimal DeferredTaxableIncome { get; set; }

        [Column("TotalTaxableSalary")]
        public decimal TaxableIncome { get; set; }

        [Column("TotalEmpWithholdingTax")]
        public decimal WithholdingTax { get; set; }

        [Column("TotalEmpSSS")]
        public decimal SssEmployeeShare { get; set; }

        [Column("TotalCompSSS")]
        public decimal SssEmployerShare { get; set; }

        [Column("TotalEmpPhilhealth")]
        public decimal PhilHealthEmployeeShare { get; set; }

        [Column("TotalCompPhilhealth")]
        public decimal PhilHealthEmployerShare { get; set; }

        [Column("TotalEmpHDMF")]
        public decimal HdmfEmployeeShare { get; set; }

        [Column("TotalCompHDMF")]
        public decimal HdmfEmployerShare { get; set; }

        public decimal TotalLoans { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        public virtual ICollection<Adjustment> Adjustments { get; set; }

        public virtual ICollection<ActualAdjustment> ActualAdjustments { get; set; }

        public virtual ICollection<AllowanceItem> AllowanceItems { get; set; }

        public virtual ICollection<LeaveTransaction> LeaveTransactions { get; set; }

        public virtual ICollection<LoanTransaction> LoanTransactions { get; set; }

        public virtual ICollection<PaystubEmail> PaystubEmails { get; set; }

        public virtual ICollection<PaystubEmailHistory> PaystubEmailHistories { get; set; }

        public virtual ICollection<PaystubItem> PaystubItems { get; set; }

        public virtual ThirteenthMonthPay ThirteenthMonthPay { get; set; }

        public virtual PaystubActual Actual { get; set; }

        // TODO: try to remove this
        [NotMapped]
        public decimal Ecola { get; set; }

        public decimal GrandTotalAllowance => TotalNonTaxableAllowance + TotalTaxableAllowance;

        public decimal NetDeductions => GovernmentDeductions + TotalLoans + WithholdingTax;

        public decimal GovernmentDeductions => SssEmployeeShare + PhilHealthEmployeeShare + HdmfEmployeeShare;

        public decimal TotalRestDayHours => RestDayHours + SpecialHolidayRestDayHours + RegularHolidayRestDayHours;

        public decimal RegularHoursAndTotalRestDay => RegularHours + TotalRestDayHours;

        public decimal TotalOvertimeHours =>
            OvertimeHours +
            RestDayOTHours +
            SpecialHolidayOTHours +
            RegularHolidayOTHours +
            SpecialHolidayRestDayOTHours +
            RegularHolidayRestDayOTHours;

        public decimal TotalWorkedHoursWithoutOvertimeAndLeave =>
            RegularHoursAndTotalRestDay +
            SpecialHolidayHours +
            RegularHolidayHours;

        public decimal TotalWorkedHoursWithoutLeave => TotalWorkedHoursWithoutOvertimeAndLeave + TotalOvertimeHours;

        public virtual decimal TotalDaysPayWithOutOvertimeAndLeave =>
            RegularPay +
            RestDayPay +
            SpecialHolidayPay +
            RegularHolidayPay +
            SpecialHolidayRestDayPay +
            RegularHolidayRestDayPay;

        public decimal TotalDeductionAdjustments =>
            Adjustments.Where(a => a.Amount < 0).Sum(a => a.Amount) +
            ActualAdjustments.Where(a => a.Amount < 0).Sum(a => a.Amount);

        public decimal TotalAdditionAdjustments =>
            Adjustments.Where(a => a.Amount > 0).Sum(a => a.Amount) +
            ActualAdjustments.Where(a => a.Amount > 0).Sum(a => a.Amount);

        public Paystub()
        {
            Adjustments = new List<Adjustment>();
            ActualAdjustments = new List<ActualAdjustment>();
            PaystubItems = new List<PaystubItem>();

            AllowanceItems = new List<AllowanceItem>();
            LeaveTransactions = new List<LeaveTransaction>();
            LoanTransactions = new List<LoanTransaction>();
        }

        public virtual ICollection<LoanPaymentFromThirteenthMonthPay> LoanPaymentFromThirteenthMonthPays { get; set; }

        #region Composite Keys

        public class EmployeeCompositeKey
        {
            public int EmployeeId { get; set; }
            public int PayPeriodId { get; set; }

            public EmployeeCompositeKey(int employeeId, int payPeriodId)
            {
                EmployeeId = employeeId;
                PayPeriodId = payPeriodId;
            }
        }

        public class DateCompositeKey
        {
            public int OrganizationId { get; set; }
            public DateTime PayFromDate { get; set; }
            public DateTime PayToDate { get; set; }

            public DateCompositeKey(int organizationId, DateTime payFromDate, DateTime payToDate)
            {
                OrganizationId = organizationId;
                PayFromDate = payFromDate;
                PayToDate = payToDate;
            }
        }

        #endregion Composite Keys
    }
}
