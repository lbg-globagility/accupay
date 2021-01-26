using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("breaktimebracket")]
    public class BreakTimeBracket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public decimal ShiftDuration { get; set; }

        public decimal BreakDuration { get; set; }

        public int DivisionID { get; set; }

        [ForeignKey("DivisionID")]
        public virtual Division Division { get; set; }
    }
}