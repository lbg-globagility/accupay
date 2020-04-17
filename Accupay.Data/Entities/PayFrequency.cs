using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("payfrequency")]
    public class PayFrequency
    {
        public const string WeeklyType = "WEEKLY";

        public const string SemiMonthlyType = "SEMI-MONTHLY";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        [Column("PayFrequencyType")]
        public string Type { get; set; }

        public bool IsWeekly
        {
            get
            {
                return Type.ToUpper() == WeeklyType;
            }
        }

        public bool IsSemiMonthly
        {
            get
            {
                return Type.ToUpper() == SemiMonthlyType;
            }
        }
    }
}