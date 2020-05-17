using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PayPeriodRepository
    {
        private readonly PayrollContext _context;

        public PayPeriodRepository(PayrollContext context)
        {
            _context = context;
        }

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
            return _context.PayPeriods.FirstOrDefault(x => x.RowID == id);
        }

        public async Task<PayPeriod> GetByIdAsync(int id)
        {
            return await _context.PayPeriods.FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async Task<PayPeriod> GetCurrentProcessing(int organizationId)
        {
            return await _context.PayPeriods.
                            Include(x => x.Paystubs).
                            Where(p => p.IsClosed == false).
                            Where(p => p.OrganizationID == organizationId).
                            Where(p => p.Paystubs.Any()).
                            FirstOrDefaultAsync();
        }

        public PayPeriod GetNextPayPeriod(int payPeriodId)
        {
            var currentPayPeriod = _context.PayPeriods.FirstOrDefault(p => p.RowID == payPeriodId);

            if (currentPayPeriod == null)
                return null;

            return _context.PayPeriods.
                            Where(p => p.OrganizationID == currentPayPeriod.OrganizationID).
                            Where(p => p.PayFrequencyID == currentPayPeriod.PayFrequencyID).
                            Where(p => p.PayFromDate > currentPayPeriod.PayFromDate).
                            OrderBy(p => p.PayFromDate).
                            FirstOrDefault();
        }

        public async Task<DateTime?> GetFirstDayOfTheYear(PayPeriod currentPayPeriod,
                                                            int organizationId)
        {
            var firstPayPeriodOfTheYear = await GetFirstPayPeriodOfTheYear(currentPayPeriod,
                                                                            organizationId);

            return firstPayPeriodOfTheYear?.PayFromDate;
        }

        public async Task<PayPeriod> GetFirstPayPeriodOfTheYear(PayPeriod currentPayPeriod,
                                                                int organizationId)
        {
            var currentPayPeriodYear = currentPayPeriod?.Year;

            if (currentPayPeriodYear == null)
                return null;

            return await _context.PayPeriods.Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.Year == currentPayPeriodYear).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.IsFirstPayPeriodOfTheYear).
                                            FirstOrDefaultAsync();
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
            return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                            payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId).
                            Where(x => _context.Paystubs.Any(p => p.PayPeriodID == x.RowID)).
                            ToListAsync();
        }

        public async Task<IEnumerable<PayPeriod>> GetByPayFrequencyAsync(int organizationId, int payFrequencyId)
        {
            return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                            payFrequencyId: payFrequencyId).
                            ToListAsync();
        }

        public async Task<IEnumerable<PayPeriod>> GetByPayFrequencyAndYearAsync(int organizationId,
                                                                                int payFrequencyId,
                                                                                int year)
        {
            return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                            payFrequencyId: payFrequencyId).
                            Where(x => x.Year == year).
                            ToListAsync();
        }

        public async Task<IEnumerable<PayPeriod>> GetByYearAndPayPrequencyAsync(int organizationId,
                                                                               int year,
                                                                               int payFrequencyId)
        {
            return await CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                            payFrequencyId: payFrequencyId).
                            Where(x => x.Year == year).
                            ToListAsync();
        }

        public IEnumerable<PayPeriod> GetByMonthYearAndPayPrequency(int organizationId,
                                                                    int month,
                                                                    int year,
                                                                    int payFrequencyId)
        {
            return CreateBaseQueryByMonthYearAndPayPrequency(organizationId: organizationId,
                                                            month: month,
                                                            year: year,
                                                            payFrequencyId: payFrequencyId).
                            ToList();
        }

        public async Task<IEnumerable<PayPeriod>> GetByMonthYearAndPayPrequencyAsync(int organizationId,
                                                                                    int month,
                                                                                    int year,
                                                                                    int payFrequencyId)
        {
            return await CreateBaseQueryByMonthYearAndPayPrequency(organizationId: organizationId,
                                                                    month: month,
                                                                    year: year,
                                                                    payFrequencyId: payFrequencyId).
                            ToListAsync();
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<PayPeriod> CreateBaseQueryByMonthYearAndPayPrequency(int organizationId,
                                                                                 int month,
                                                                                 int year,
                                                                                 int payFrequencyId)
        {
            return CreateBaseQueryByPayPrequency(organizationId: organizationId,
                                                payFrequencyId: payFrequencyId).
                            Where(x => x.Year == year).
                            Where(x => x.Month == month);
        }

        private IQueryable<PayPeriod> CreateBaseQueryByPayPrequency(int organizationId,
                                                                   int payFrequencyId)
        {
            return _context.PayPeriods.
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => x.PayFrequencyID == payFrequencyId);
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

            payPeriod.IsClosed = isClosed;

            _context.Entry(payPeriod).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}