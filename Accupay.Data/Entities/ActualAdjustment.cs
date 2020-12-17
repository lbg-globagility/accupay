using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("paystubadjustmentactual")]
    public class ActualAdjustment : IAdjustment
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

        public int? ProductID { get; set; }

        public int? PaystubID { get; set; }

        [Column("PayAmount")]
        public decimal Amount { get; set; }

        public string Comment { get; set; }

        public bool IsActual { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        public bool Is13thMonthPay { get; set; }
    }
}