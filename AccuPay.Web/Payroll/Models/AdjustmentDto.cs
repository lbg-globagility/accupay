using AccuPay.Data.Entities;

namespace AccuPay.Web.Payroll
{
    public class AdjustmentDto
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public static AdjustmentDto Convert(Adjustment adjustment)
        {
            return new AdjustmentDto()
            {
                Description = adjustment.Product.PartNo,
                Amount = adjustment.Amount
            };
        }
    }
}
