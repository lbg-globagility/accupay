using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("paystubactual")]
    public class PaystubActual : BasePaystub
    {
        // This is needed even if this property is not used.
        // Specifying the foreign key is needed.
        // This can be configured on OnModelCreating instead though.
        [ForeignKey("RowID")]
        public virtual Paystub Paystub { get; set; }

        // this should not have an attribute [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // that is why this is overriden just for the attribute
        [Key]
        public int? RowID { get; set; }
    }
}