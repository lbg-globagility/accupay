using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("payrate")]
    public class PayRate : IPayrate
    {
        public const string RegularDay = "Regular Day";

        public const string SpecialNonWorkingHoliday = "Special Non-Working Holiday";

        public const string RegularHoliday = "Regular Holiday";

        public const string DoubleHoliday = "Double Holiday";

        public const string RegularDayAndSpecialHoliday = "Regular + Special Holiday";

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

        public DateTime? DayBefore { get; set; }

        public DateTime Date { get; set; }

        public string PayType { get; set; }

        public string Description { get; set; }

        [Column("PayRate")]
        public decimal CommonRate { get; set; }

        public decimal OvertimeRate { get; set; }

        public decimal NightDifferentialRate { get; set; }

        public decimal NightDifferentialOTRate { get; set; }

        public decimal RestDayRate { get; set; }

        public decimal RestDayOvertimeRate { get; set; }

        public decimal RestDayNDRate { get; set; }

        public decimal RestDayNDOTRate { get; set; }

        public bool IsRegularDay => PayType == RegularDay;

        public bool IsHoliday => IsSpecialNonWorkingHoliday | IsRegularHoliday;

        public bool IsSpecialNonWorkingHoliday => PayType == SpecialNonWorkingHoliday;

        public bool IsRegularHoliday => PayType == RegularHoliday || PayType == DoubleHoliday;

        public decimal RegularRate => CommonRate;

        public decimal NightDiffRate => NightDifferentialRate;

        public decimal NightDiffOTRate => NightDifferentialOTRate;

        public decimal RestDayOTRate => RestDayOvertimeRate;

        public bool IsDoubleHoliday => PayType == DoubleHoliday;
    }
}