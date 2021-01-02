using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanPaymentFromThirteenthMonthPayDataService
    {
        Task SaveManyAsync(PayPeriod currentPayPeriod, List<LoanPaymentFromThirteenthMonthPay> loanPaymentFromThirteenthMonthPays);
    }
}
