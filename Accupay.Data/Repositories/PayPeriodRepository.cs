using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    // TODO: use a query builder to prevent using IsSemiMonthly and other entity functions directly since it does not directly translate to sql query.
    public class PayPeriodRepository : SavableRepository<PayPeriod>
    {
        private readonly PolicyHelper _policy;

        public PayPeriodRepository(PayrollContext context, PolicyHelper policy) : base(context)
        {
            _policy = policy;
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

        /// <summary>
        /// Get the latest payperiod that was Closed or Open based on Status property. (Used in Web)
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetLatestAsync(int organizationId)
        {
            return await _context.PayPeriods
                .Where(p => p.Status != PayPeriodStatus.Pending)
                .Where(p => p.OrganizationID == organizationId)
                .OrderByDescending(p => p.PayFromDate)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// This returns the current "PROCESSING" pay period based on IsClosed and paystub count. (Used in desktop)
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetCurrentProcessing(int organizationId)
        {
            return await _context.PayPeriods
                .Include(x => x.Paystubs)
                .Where(p => p.IsClosed == false)
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Paystubs.Any())
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// This returns the current pay period based on date today regardless of the Status and IsClosed property.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetCurrentPayPeriodAsync(int organizationId)
        {
            var currentDay = DateTime.Now;
            var isFirstHalf = currentDay.Day <= 15;

            var query = _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Year == currentDay.Year)
                .Where(p => p.Month == currentDay.Month);

            if (isFirstHalf)
            {
                query = query.Where(x => x.IsFirstHalf);
            }
            else
            {
                query = query.Where(x => x.IsEndOfTheMonth);
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the pay period based on checking wether the passed date is within that pay period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetAsync(int organizationId, DateTime date)
        {
            return await _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.IsBetween(date))
                .Where(p => p.IsSemiMonthly)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the pay period based on checking if the passed cutoff start and end date matches the pay period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="cutoffDate"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetAsync(int organizationId, TimePeriod cutoffDate)
        {
            return await _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.PayFromDate == cutoffDate.Start)
                .Where(p => p.PayToDate == cutoffDate.End)
                .Where(p => p.IsSemiMonthly)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the pay period based on checking wether the passed organizationId, month, year and it's half type matches the pay period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="isFirstHalf"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetAsync(int organizationId, int month, int year, bool isFirstHalf)
        {
            // better use builder here to use encapsulated IsFirstHalf directly
            var half = isFirstHalf ? PayPeriod.FirstHalfValue : PayPeriod.EndOftheMonthValue;
            return await _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Month == month)
                .Where(p => p.Year == year)
                .Where(p => p.Half == half)
                .FirstOrDefaultAsync();
        }

        public PayPeriod GetNextPayPeriod(int payPeriodId)
        {
            var currentPayPeriod = _context.PayPeriods.FirstOrDefault(p => p.RowID == payPeriodId);

            if (currentPayPeriod == null)
                return null;

            return _context.PayPeriods
                .Where(p => p.OrganizationID == currentPayPeriod.OrganizationID)
                .Where(p => p.PayFrequencyID == currentPayPeriod.PayFrequencyID)
                .Where(p => p.PayFromDate > currentPayPeriod.PayFromDate)
                .OrderBy(p => p.PayFromDate)
                .FirstOrDefault();
        }

        public async Task<DateTime?> GetFirstDayOfTheYear(PayPeriod currentPayPeriod, int organizationId)
        {
            var firstPayPeriodOfTheYear = await GetFirstPayPeriodOfTheYear(currentPayPeriod, organizationId);

            return firstPayPeriodOfTheYear?.PayFromDate;
        }

        public async Task<PayPeriod> GetFirstPayPeriodOfTheYear(PayPeriod currentPayPeriod, int organizationId)
        {
            var currentPayPeriodYear = currentPayPeriod?.Year;

            if (currentPayPeriodYear == null)
                return null;

            return await _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsSemiMonthly)
                .Where(p => p.IsFirstPayPeriodOfTheYear)
                .FirstOrDefaultAsync();
        }

        public async Task<DateTime?> GetLastDayOfTheYear(PayPeriod currentPayPeriod, int organizationId)
        {
            var currentPayPeriodYear = currentPayPeriod?.Year;

            if (currentPayPeriodYear == null)
                return null;

            return await _context.PayPeriods
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsSemiMonthly)
                .Where(p => p.IsLastPayPeriodOfTheYear)
                .Select(p => p.PayToDate)
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<PayPeriod>> GetAllSemiMonthlyThatHasPaystubsAsync(int organizationId)
        {
            return await CreateBaseQueryByPayPrequency(
                    organizationId: organizationId,
                    payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId)
                .Where(x => _context.Paystubs.Any(p => p.PayPeriodID == x.RowID))
                .ToListAsync();
        }

        public async Task<ICollection<PayPeriod>> GetByYearAndPayPrequencyAsync(
            int organizationId,
            int year,
            int payFrequencyId)
        {
            return await CreateBaseQueryByPayPrequency(
                    organizationId: organizationId,
                    payFrequencyId: payFrequencyId)
                .Where(x => x.Year == year)
                .ToListAsync();
        }

        public ICollection<PayPeriod> GetByMonthYearAndPayPrequency(
            int organizationId,
            int month,
            int year,
            int payFrequencyId)
        {
            return CreateBaseQueryByMonthYearAndPayPrequency(
                    organizationId: organizationId,
                    month: month,
                    year: year,
                    payFrequencyId: payFrequencyId)
                .ToList();
        }

        public async Task<ICollection<PayPeriod>> GetByMonthYearAndPayPrequencyAsync(
            int organizationId,
            int month,
            int year,
            int payFrequencyId)
        {
            return await CreateBaseQueryByMonthYearAndPayPrequency(
                    organizationId: organizationId,
                    month: month,
                    year: year,
                    payFrequencyId: payFrequencyId)
                .ToListAsync();
        }

        public async Task<ICollection<PayPeriod>> GetYearlyPayPeriodsAsync(int organizationId, int year)
        {
            var yearlyPayPeriods = new List<PayPeriod>();

            var payPeriods = await _context.PayPeriods
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.Year == year)
                .ToListAsync();

            var months = Enumerable.Range(1, 12);

            foreach (int month in months)
            {
                var firstHalf = payPeriods.FirstOrDefault(x => x.Month == month && x.IsSemiMonthly && x.IsFirstHalf);
                var endOfTheMonth = payPeriods.FirstOrDefault(x => x.Month == month && x.IsSemiMonthly && x.IsEndOfTheMonth);

                if (firstHalf == null)
                {
                    firstHalf = PayPeriod.NewPayPeriod(organizationId, month, year, isFirstHalf: true, _policy);
                }

                if (endOfTheMonth == null)
                {
                    endOfTheMonth = PayPeriod.NewPayPeriod(organizationId, month, year, isFirstHalf: false, _policy);
                }

                yearlyPayPeriods.Add(firstHalf);
                yearlyPayPeriods.Add(endOfTheMonth);
            }

            return yearlyPayPeriods;
        }

        public async Task<PaginatedListResult<PayPeriod>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.PayPeriods
                .Where(t => t.OrganizationID == organizationId)
                .Where(t => t.Status != PayPeriodStatus.Pending)
                .Where(t => t.PayFrequencyID == PayrollTools.PayFrequencySemiMonthlyId)
                .OrderByDescending(t => t.PayFromDate)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.PayFromDate.ToString(), searchTerm) ||
                    EF.Functions.Like(x.PayToDate.ToString(), searchTerm));
            }

            var payperiods = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<PayPeriod>(payperiods, count);
        }

        #endregion List of entities

        #endregion Queries

        private IQueryable<PayPeriod> CreateBaseQueryByMonthYearAndPayPrequency(
            int organizationId,
            int month,
            int year,
            int payFrequencyId)
        {
            return CreateBaseQueryByPayPrequency(
                    organizationId: organizationId,
                    payFrequencyId: payFrequencyId)
                .Where(x => x.Year == year)
                .Where(x => x.Month == month);
        }

        private IQueryable<PayPeriod> CreateBaseQueryByPayPrequency(
            int organizationId,
            int payFrequencyId)
        {
            return _context.PayPeriods
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.PayFrequencyID == payFrequencyId);
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