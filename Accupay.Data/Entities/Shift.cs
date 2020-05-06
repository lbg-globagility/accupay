using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("shift")]
    public class Shift
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

        public TimeSpan? BreaktimeFrom { get; set; }

        public TimeSpan? BreaktimeTo { get; set; }

        public decimal DivisorToDailyRate { get; set; }

        public decimal ShiftHours { get; set; }

        public decimal WorkHours { get; set; }

        public bool HasBreaktime => BreaktimeFrom.HasValue && BreaktimeTo.HasValue;

        public Shift()
        {
        }

        public Shift(TimeSpan timeFrom, TimeSpan timeTo)
        {
            this.TimeFrom = timeFrom;
            this.TimeTo = timeTo;
        }

        public Shift(TimeSpan timeFrom, TimeSpan timeTo, TimeSpan breaktimeFrom, TimeSpan breaktimeTo) : this(timeFrom, timeTo)
        {
            this.BreaktimeFrom = breaktimeFrom;
            this.BreaktimeTo = breaktimeTo;
        }
    }
}