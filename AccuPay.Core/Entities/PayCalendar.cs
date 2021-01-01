using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("calendar")]
    public class PayCalendar
    {
        public const string DefaultName = "[Default]";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public static PayCalendar NewCalendar(string calendarName)
        {
            return new PayCalendar()
            {
                IsDefault = false,
                Name = calendarName?.Trim(),
            };
        }

        public static PayCalendar CreateDefaultCalendar()
        {
            return new PayCalendar()
            {
                IsDefault = true,
                Name = DefaultName,
            };
        }
    }
}
