using AccuPay.Data.Entities;

namespace AccuPay.Web.Loans.LoanType
{
    public class LoanTypeDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        internal static LoanTypeDto Convert(Product loanType)
        {
            return new LoanTypeDto() { Id = loanType.RowID.Value, Name = loanType.PartNo };
        }
    }
}
