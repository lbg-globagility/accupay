using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("agency")]
    public class Agency
    {
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

        [Column("AgencyName")]
        public string Name { get; set; }

        [Column("AgencyFee")]
        public decimal Fee { get; set; }

        public bool IsActive { get; set; }
    }
}