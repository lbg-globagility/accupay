using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PayPeriodRepository
    {
        #region Queries

        #region Single entity

        public PayPeriod GetById(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return context.PayPeriods.FirstOrDefault(x => x.RowID == id);
            }
        }

        public async Task<PayPeriod> GetByIdAsync(int id)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PayPeriods.FirstOrDefaultAsync(x => x.RowID == id);
            }
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<PayPeriod>> GetAllSemiMonthlyAsync(int organizationId)
        {
            return await GetByPayFrequencyAsync(organizationId: organizationId,
                                                payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId);
        }

        public async Task<IEnumerable<PayPeriod>> GetAllSemiMonthlyThatHasPaystubsAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                                payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId,
                                                context: context).
                                Where(x => context.Paystubs.Any(p => p.PayPeriodID == x.RowID)).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<PayPeriod>> GetByPayFrequencyAsync(int organizationId, int payFrequencyId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                                payFrequencyId: payFrequencyId,
                                                context: context).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<PayPeriod>> GetByYearAndPayPrequencyAsync(int organizationId,
                                                                               int year,
                                                                               int payFrequencyId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                                payFrequencyId: payFrequencyId,
                                                context: context).
                                Where(x => x.Year == year).
                                ToListAsync();
            }
        }

        public IEnumerable<PayPeriod> GetByMonthYearAndPayPrequency(int organizationId,
                                                                    int month,
                                                                    int year,
                                                                    int payFrequencyId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return CreateBaseQueryByMonthYearAndPayPrequency(organizationId: organizationId,
                                                                month: month,
                                                                year: year,
                                                                payFrequencyId: payFrequencyId,
                                                                context: context).
                                ToList();
            }
        }

        public async Task<IEnumerable<PayPeriod>> GetByMonthYearAndPayPrequencyAsync(int organizationId,
                                                                                    int month,
                                                                                    int year,
                                                                                    int payFrequencyId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByMonthYearAndPayPrequency(organizationId: organizationId,
                                                                        month: month,
                                                                        year: year,
                                                                        payFrequencyId: payFrequencyId,
                                                                        context: context).
                                ToListAsync();
            }
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<PayPeriod> CreateBaseQueryByPayPrequency(int organizationId,
                                                                    int payFrequencyId,
                                                                    PayrollContext context)
        {
            return context.PayPeriods.
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.PayFrequencyID == payFrequencyId);
        }

        private IQueryable<PayPeriod> CreateBaseQueryByMonthYearAndPayPrequency(int organizationId,
                                                                                int month,
                                                                                int year,
                                                                                int payFrequencyId,
                                                                                PayrollContext context)
        {
            return CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                                payFrequencyId: payFrequencyId,
                                                context: context).
                            Where(x => x.Year == year).
                            Where(x => x.Month == month);
        }
    }
}