using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("calendarday")]
    public class CalendarDay : IPayrate
    {
        [Key]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated { get; set; }

        public int? UpdatedBy { get; set; }

        public int? CalendarID { get; set; }

        public int? DayTypeID { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public virtual DayType DayType { get; set; }

        public decimal RegularRate => DayType.RegularRate;

        public decimal OvertimeRate => DayType.OvertimeRate;

        public decimal NightDiffRate => DayType.NightDiffRate;

        public decimal NightDiffOTRate => DayType.NightDiffOTRate;

        public decimal RestDayRate => DayType.RestDayRate;

        public decimal RestDayOTRate => DayType.RestDayOTRate;

        public decimal RestDayNDRate => DayType.RestDayNDRate;

        public decimal RestDayNDOTRate => DayType.RestDayNDOTRate;

        public bool IsRegularDay => DayType?.DayConsideredAs == "Regular Day" || DayType is null;

        public bool IsSpecialNonWorkingHoliday => DayType?.DayConsideredAs == "Special Non-Working Holiday";

        public bool IsRegularHoliday => DayType?.DayConsideredAs == "Regular Holiday" || DayType?.DayConsideredAs == "Double Holiday";

        public bool IsDoubleHoliday => DayType?.DayConsideredAs == "Double Holiday";

        public bool IsHoliday => IsRegularHoliday || IsSpecialNonWorkingHoliday;
    }
}