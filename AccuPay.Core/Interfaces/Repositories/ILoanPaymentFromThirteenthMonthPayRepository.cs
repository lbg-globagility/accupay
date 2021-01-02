using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanPaymentFromThirteenthMonthPayRepository
    {
        Task SaveManyAsync(List<LoanPaymentFromThirteenthMonthPay> LoanPaymentFromThirteenthMonthPays);
    }
}
