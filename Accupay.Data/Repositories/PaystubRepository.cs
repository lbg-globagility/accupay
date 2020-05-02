using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubRepository
    {
        public async Task<IEnumerable<Paystub>> GetPreviousCutOffPaystubs(
                                            DateTime currentCuttOffStart,
                                            int organizationId)
        {
            var previousCutoffEnd = currentCuttOffStart.AddDays(-1);

            using (var context = new PayrollContext())
            {
                return await context.Paystubs.
                                Where(x => x.PayToDate == previousCutoffEnd).
                                Where(x => x.OrganizationID == organizationId).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<Paystub>> GetByPayPeriodWithEmployeeAsync(int payPeriodId)
        {
            using (var context = new PayrollContext())
            {
                return await context.Paystubs.
                                Include(x => x.Employee).
                                Where(x => x.PayPeriodID == payPeriodId).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<AllowanceItem>> GetAllowanceItems(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                var paystub = await context.Paystubs.
                                Include(x => x.AllowanceItems).
                                    ThenInclude(x => x.Allowance).
                                        ThenInclude(x => x.Product).
                                Where(x => x.RowID == paystubId).
                                FirstOrDefaultAsync();

                return paystub.AllowanceItems;
            }
        }

        public async Task<IEnumerable<LoanTransaction>> GetLoanTransanctions(int paystubId)
        {
            using (var context = new PayrollContext())
            {
                var paystub = await context.Paystubs.
                                Include(x => x.LoanTransactions).
                                    ThenInclude(x => x.LoanSchedule).
                                        ThenInclude(x => x.LoanType).
                                Where(x => x.RowID == paystubId).
                                FirstOrDefaultAsync();

                return paystub.LoanTransactions;
            }
        }
    }
}