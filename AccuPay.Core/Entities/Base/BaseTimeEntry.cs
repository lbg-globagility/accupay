using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    public abstract class BaseTimeEntry : BaseEntity
    {
        public int? OrganizationID { get; set; }

        public int? EmployeeID { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }

        [Column("RegularHoursAmount")]
        public decimal RegularPay { get; set; }

        [Column("OvertimeHoursAmount")]
        public decimal OvertimePay { get; set; }

        [Column("BasicDayPay")]
        public decimal BasicDayPay { get; set; }

        [Column("NightDiffHoursAmount")]
        public decimal NightDiffPay { get; set; }

        [Column("NightDiffOTHoursAmount")]
        public decimal NightDiffOTPay { get; set; }

        [Column("RestDayAmount")]
        public decimal RestDayPay { get; set; }

        public decimal RestDayOTPay { get; set; }

        [Column("Leavepayment")]
        public decimal LeavePay { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayPay { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        [Column("HoursLateAmount")]
        public decimal LateDeduction { get; set; }

        [Column("UndertimeHoursAmount")]
        public decimal UndertimeDeduction { get; set; }

        [Column("Absent")]
        public decimal AbsentDeduction { get; set; }

        [Column("TotalDayPay")]
        public decimal TotalDayPay { get; set; }
    }
}
