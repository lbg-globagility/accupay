using AccuPay.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("paystubadjustment")]
    public class Adjustment : OrganizationalEntity, IAdjustment
    {
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

        public IAdjustment Clone()
        {
            return new Adjustment()
            {
                RowID = RowID,
                OrganizationID = OrganizationID,
                CreatedBy = CreatedBy,
                LastUpdBy = LastUpdBy,
                ProductID = ProductID,
                PaystubID = PaystubID,
                Amount = Amount,
                Comment = Comment,
                IsActual = IsActual,
                Paystub = Paystub,
                Product = Product,
                Is13thMonthPay = Is13thMonthPay,
            };
        }

        public static Adjustment NewAdjustment(decimal amount,
            string comment,
            int? productId = null,
            int? paystubId = null)
        {
            return new Adjustment()
            {
                ProductID = productId,
                PaystubID = paystubId,
                Amount = amount,
                Comment = comment,
                IsActual = false
            };
        }
    }
}
