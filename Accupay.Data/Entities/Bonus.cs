using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employeebonus")]
    public class Bonus
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

        public int? EmployeeID { get; set; }

        public int? ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [Column("EffectiveStartDate")]
        public DateTime EffectiveStartDate { get; set; }

        [Column("EffectiveEndDate")]
        public DateTime EffectiveEndDate { get; set; }

        [Column("AllowanceFrequency")]
        public string AllowanceFrequency { get; set; }

        [Column("TaxableFlag")]
        public string TaxableFlag { get; set; }

        [Column("BonusAmount")]
        public decimal? BonusAmount { get; set; }

        [Column("RemainingBalance")]
        public decimal? RemainingBalance { get; set; }

        public string BonusType
        {
            get
            {
                return Product?.Name;
            }
        }
    }
}
