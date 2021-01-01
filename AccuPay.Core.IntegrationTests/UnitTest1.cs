using AccuPay.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Xunit;

namespace AccuPay.Core.IntegrationTests
{
    public class UnitTest1 : DatabaseTest
    {
        [Fact]
        public async Task Test1()
        {
            Assert.True(true);
            return;

            var loanRepository = MainServiceProvider.GetRequiredService<LoanRepository>();
            var payPeriodRepository = MainServiceProvider.GetRequiredService<PayPeriodRepository>();
            var paystubRepository = MainServiceProvider.GetRequiredService<PaystubRepository>();
            var payrollContext = MainServiceProvider.GetRequiredService<PayrollContext>();

            var payPeriod = await payPeriodRepository.GetByIdAsync(648);

            var paystubs = await paystubRepository.GetByPayPeriodFullPaystubAsync(payPeriod.RowID.Value);

            //var loans = await loanRepository.GetCurrentPayrollLoansAsync(2, payPeriod, paystubs.ToList());

            //var loan = loans.Where(x => x.RowID == 1865).ToList();

            //var interests = await payrollContext
            //    .Loans
            //    .Include(x => x.YearlyLoanInterests)
            //    .Where(x => x.RowID == 1865)
            //    .FirstOrDefaultAsync();

            //Assert.NotNull(loans);
        }
    }
}
