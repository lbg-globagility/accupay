using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("paystubitem")]
    public class PaystubItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual int? PayStubID { get; set; }

        public virtual int? ProductID { get; set; }

        public virtual decimal PayAmount { get; set; }

        // this might throw an error when accessed.
        // it threw an error when used in Gotesco project.
        // currently no code references this property so this is not an issue.
        // String is the recommended data type for Undeclared
        public virtual char Undeclared { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("PayStubID")]
        public virtual Paystub Paystub { get; set; }
    }
}