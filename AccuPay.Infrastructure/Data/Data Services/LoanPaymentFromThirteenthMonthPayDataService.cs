using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LoanPaymentFromThirteenthMonthPayDataService : ILoanPaymentFromThirteenthMonthPayDataService
    {
        private readonly ILoanPaymentFromThirteenthMonthPayRepository _loanPaymentFromThirteenthMonthPayRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;

        public LoanPaymentFromThirteenthMonthPayDataService(
            ILoanPaymentFromThirteenthMonthPayRepository loanPaymentFromThirteenthMonthPayRepository,
            IPayPeriodRepository payPeriodRepository)
        {
            _loanPaymentFromThirteenthMonthPayRepository = loanPaymentFromThirteenthMonthPayRepository;
            _payPeriodRepository = payPeriodRepository;
        }

        public async Task SaveManyAsync(PayPeriod currentPayPeriod, List<LoanPaymentFromThirteenthMonthPay> loanPaymentFromThirteenthMonthPays)
        {
            var succeedingPeriods = await _payPeriodRepository.GetPeriodsFromThisPeriodOnwardsAsync(currentPayPeriod);

            var paystubId = loanPaymentFromThirteenthMonthPays.FirstOrDefault()?.PaystubId;

            var succeedingPeriodsWithPaystubsWithLoanTransactions = succeedingPeriods.
                Where(pp => pp.Paystubs.Any(ps => ps.RowID == paystubId && ps.LoanTransactions.Any()));

            if (succeedingPeriodsWithPaystubsWithLoanTransactions.Any())
            {
                throw new BusinessLogicException($"{succeedingPeriodsWithPaystubsWithLoanTransactions.Count()} period(s) had passed and Loan Transactions were recorded already, therefore saving these changes will affect the Loan Balances of existing later periods.");
            }

            await _loanPaymentFromThirteenthMonthPayRepository.SaveManyAsync(loanPaymentFromThirteenthMonthPays);
        }
    }
}
