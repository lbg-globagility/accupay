using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("paystub")]
    public class Paystub : IPaystub
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? PayPeriodID { get; set; }

        public int? EmployeeID { get; set; }

        public int? TimeEntryID { get; set; }

        public DateTime PayFromdate { get; set; }

        public DateTime PayToDate { get; set; }

        public decimal BasicHours { get; set; }

        public decimal BasicPay { get; set; }

        public decimal RegularHours { get; set; }

        public decimal RegularPay { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimePay { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffPay { get; set; }

        public decimal NightDiffOvertimeHours { get; set; }

        public decimal NightDiffOvertimePay { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayPay { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal RestDayOTPay { get; set; }

        public decimal LeaveHours { get; set; }

        public decimal LeavePay { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        [Obsolete]
        public decimal HolidayPay { get; set; }

        public decimal LateHours { get; set; }

        public decimal LateDeduction { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal UndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal AbsenceDeduction { get; set; }

        [Column("WorkPay")]
        public decimal TotalEarnings { get; set; }

        public decimal TotalBonus { get; set; }

        public decimal TotalAllowance { get; set; }

        public decimal TotalTaxableAllowance { get; set; }

        [Column("TotalGrossSalary")]
        public decimal GrossPay { get; set; }

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

        public decimal TotalVacationDaysLeft { get; set; }

        public decimal TotalUndeclaredSalary { get; set; }

        public decimal TotalLoans { get; set; }

        public decimal TotalAdjustments { get; set; }

        [Column("TotalNetSalary")]
        public decimal NetPay { get; set; }

        public bool ThirteenthMonthInclusion { get; set; }

        public bool FirstTimeSalary { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        //public virtual ICollection<Adjustment> Adjustments { get; set; }

        //public virtual ICollection<ActualAdjustment> ActualAdjustments { get; set; }

        //public virtual ICollection<PaystubItem> PaystubItems { get; set; }

        //public virtual ICollection<AllowanceItem> AllowanceItems { get; set; }

        public virtual ThirteenthMonthPay ThirteenthMonthPay { get; set; }

        //public virtual PaystubActual Actual { get; set; }

        public decimal SpecialHolidayRestDayPay { get; set; }

        public decimal RegularHolidayRestDayPay { get; set; }
    }
}