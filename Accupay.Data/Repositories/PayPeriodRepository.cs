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
        #region CRUD

        public async Task OpenAsync(int id)
        {
            await ToggleCloseAsync(id, isClosed: false);
        }

        public async Task CloseAsync(int id)
        {
            await ToggleCloseAsync(id, isClosed: true);
        }

        #endregion CRUD

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

        public async Task<PayPeriod> GetCurrentProcessing(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.PayPeriods.
                                Include(x => x.Paystubs).
                                Where(p => p.IsClosed == false).
                                Where(p => p.OrganizationID == organizationId).
                                Where(p => p.Paystubs.Any()).
                                FirstOrDefaultAsync();
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

        /// <summary>
        /// Set the IsClosed property of the pay period to the specified status. We could have just
        /// created an UpdateAsync and let the user call that but we want to encapsulate how can client
        /// codes manipulate PayPeriod data. They can easily change the Organization or PayFrequency or
        /// dates if we let them update anything. Better solution is set the entity properties to readonly
        /// and add an entity method like PayPeriod.Close() and PayPeriod.Open() to put the encapsulation
        /// on the domain level as DDD suggests.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isClosed"></param>
        /// <returns></returns>
        private async Task ToggleCloseAsync(int id, bool isClosed)
        {
            var payPeriod = await GetByIdAsync(id);

            if (payPeriod == null) return;

            using (PayrollContext context = new PayrollContext())
            {
                payPeriod.IsClosed = isClosed;

                context.Entry(payPeriod).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }
    }
}