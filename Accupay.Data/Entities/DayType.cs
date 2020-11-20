using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("daytype")]
    public class DayType
    {
        private DayType()
        {
            // create a factory method to create new Daytypes
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Updated { get; set; }

        public int? UpdatedBy { get; set; }

        public string Name { get; set; }

        public decimal RegularRate { get; set; }

        public decimal OvertimeRate { get; set; }

        public decimal NightDiffRate { get; set; }

        public decimal NightDiffOTRate { get; set; }

        public decimal RestDayRate { get; set; }

        public decimal RestDayOTRate { get; set; }

        public decimal RestDayNDRate { get; set; }

        public decimal RestDayNDOTRate { get; set; }

        public string DayConsideredAs { get; set; }

        public static DayType CreateRegularDayType()
        {
            return new DayType()
            {
                Name = CalendarConstant.RegularDay,
                RegularRate = 1,
                OvertimeRate = 1.25M,
                NightDiffRate = 1.1M,
                NightDiffOTRate = 1.375M,
                RestDayRate = 1.3M,
                RestDayOTRate = 1.69M,
                RestDayNDRate = 1.43M,
                RestDayNDOTRate = 1.859M,
                DayConsideredAs = CalendarConstant.RegularDay
            };
        }
    }
}
