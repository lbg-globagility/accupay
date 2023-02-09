using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("dates")]
    public class DateEntity
    {
        [Key]
        [Column("DateValue")]
        public DateTime Value { get; set; }

        public int Month => Value.Month;

        public int Year => Value.Year;

        public DayOfWeek DayOfWeek => Value.Date.DayOfWeek;

        public string DayName => DayOfWeek.ToString();

        public string DayNameToLower => DayOfWeek.ToString().ToLower();
        //Sunday = 0,
        //Monday = 1,
        //Tuesday = 2,
        //Wednesday = 3,
        //Thursday = 4,
        //Friday = 5,
        //Saturday = 6
    }
}
