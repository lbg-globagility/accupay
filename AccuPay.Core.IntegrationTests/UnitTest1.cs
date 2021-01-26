using AccuPay.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;

namespace AccuPay.Core.IntegrationTests
{
    public class UnitTest1 : DatabaseTest
    {
        [Test]
        public async Task Test1()
        {
            Assert.True(true);

            var loanRepository = MainServiceProvider.GetRequiredService<ILoanRepository>();
            var payPeriodRepository = MainServiceProvider.GetRequiredService<IPayPeriodRepository>();
            var paystubRepository = MainServiceProvider.GetRequiredService<IPaystubRepository>();

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
