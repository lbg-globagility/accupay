using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("agencyfee")]
    public class AgencyFee
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

        public int? AgencyID { get; set; }

        public int? EmployeeID { get; set; }

        public int? DivisionID { get; set; }

        public int? TimeEntryID { get; set; }

        [Column("TimeEntryDate")]
        public DateTime Date { get; set; }

        [Column("DailyFee")]
        public decimal Amount { get; set; }
    }
}