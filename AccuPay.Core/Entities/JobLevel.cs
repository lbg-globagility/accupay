using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("joblevel")]
    public class JobLevel
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? JobCategoryID { get; set; }

        public string Name { get; set; }

        public int Points { get; set; }

        public decimal SalaryRangeFrom { get; set; }

        public decimal SalaryRangeTo { get; set; }

        [ForeignKey("JobCategoryID")]
        public virtual JobCategory JobCategory { get; set; }
    }
}