using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("divisionminimumwage")]
    public class DivisionMinimumWage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public int? DivisionID { get; set; }

        public decimal Amount { get; set; }

        public DateTime EffectiveDateFrom { get; set; }

        public DateTime EffectiveDateTo { get; set; }
    }
}