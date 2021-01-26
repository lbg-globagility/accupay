using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("thirteenthmonthpay")]
    public class ThirteenthMonthPay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual decimal BasicPay { get; set; }

        public virtual decimal Amount { get; set; }

        [Key]
        public int? PaystubID { get; set; }

        public virtual Paystub Paystub { get; set; }
    }
}