using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Policies;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
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

        #region Queries

        #region Single entity

        /// <summary>
        /// Get the latest payperiod that was Closed or Open based on Status property. (Used in Web)
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetLatestAsync(int organizationId)
        {
            var recentPeriod = await CreateBaseQuery(organizationId)
                .Where(p => p.Status != PayPeriodStatus.Pending)
                .OrderByDescending(p => p.PayFromDate)
                .FirstOrDefaultAsync();

            if (recentPeriod == null)
            {
                var currentYear = DateTime.UtcNow.Year;

                return await CreateBaseQuery(organizationId)
                    .Where(p => p.Year == currentYear)
                    .OrderBy(p => p.PayFromDate)
                    .FirstOrDefaultAsync();
            }

            return recentPeriod;
        }

        public async Task<PayPeriod> GetCurrentOpenAsync(int organizationId)
        {
            IQueryable<PayPeriod> query = CreateBaseQueryGetCurrentOpen(organizationId);

            return await query.FirstOrDefaultAsync();
        }

        public PayPeriod GetCurrentOpen(int organizationId)
        {
            IQueryable<PayPeriod> query = CreateBaseQueryGetCurrentOpen(organizationId);

            return query.FirstOrDefault();
        }

        public async Task<ICollection<PayPeriod>> GetClosedPayPeriodsAsync(int organizationId, TimePeriod dateRange = null)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(p => p.Status == PayPeriodStatus.Closed);

            if (dateRange != null)
            {
                var cutOff = GetCutOffPeriodUsingDefault(dateRange);

                query = query
                    .Where(x => x.PayFromDate >= cutOff.Start)
                    .Where(x => x.PayToDate <= cutOff.End);
            }

            return await query.ToListAsync();
        }

        public async Task<bool> HasClosedPayPeriodAfterDateAsync(int organizationId, DateTime date)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(p => p.Status == PayPeriodStatus.Closed);

            var cutOffStart = GetCutOffPeriodUsingDefault(new TimePeriod(date, date)).Start;
            query = query.Where(x => x.PayFromDate >= cutOffStart);

            return await query.AnyAsync();
        }

        public async Task<PayPeriod> GetCurrentPayPeriodAsync(int organizationId)
        {
            var currentDay = DateTime.Now;
            var isFirstHalf = currentDay.Day <= 15;

            var query = CreateBaseQuery(organizationId)
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
            return await CreateBaseQuery(organizationId)
                .Where(p => p.IsBetween(date))
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
            return await CreateBaseQuery(organizationId)
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

            return await CreateBaseQuery(organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsFirstPayPeriodOfTheYear)
                .FirstOrDefaultAsync();
        }

        public async Task<DateTime?> GetLastDayOfTheYear(PayPeriod currentPayPeriod, int organizationId)
        {
            var currentPayPeriodYear = currentPayPeriod?.Year;

            if (currentPayPeriodYear == null)
                return null;

            return await CreateBaseQuery(organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsLastPayPeriodOfTheYear)
                .Select(p => p.PayToDate)
                .FirstOrDefaultAsync();
        }

        #endregion Single entity

        #region List of entities

        public async Task<ICollection<PayPeriod>> GetAllSemiMonthlyThatHasPaystubsAsync(int organizationId)
        {
            return await CreateBaseQuery(organizationId)
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

            var payPeriods = await CreateBaseQuery(organizationId)
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

        public async Task<PaginatedList<PayPeriod>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = CreateBaseQuery(organizationId)
                .Where(t => t.Status != PayPeriodStatus.Pending)
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

            return new PaginatedList<PayPeriod>(payperiods, count);
        }

        #endregion List of entities

        #endregion Queries

        #region Private helper methods

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

        private IQueryable<PayPeriod> CreateBaseQuery(int organizationId)
        {
            return _context.PayPeriods
                .Where(x => x.OrganizationID == organizationId)
                .Where(p => p.PayFrequencyID == PayrollTools.PayFrequencySemiMonthlyId);
        }

        private IQueryable<PayPeriod> CreateBaseQueryGetCurrentOpen(int organizationId)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(p => p.Status == PayPeriodStatus.Open)
                .OrderByDescending(p => p.PayFromDate);

            return query;
        }

        private TimePeriod GetCutOffPeriodUsingDefault(TimePeriod dateRange)
        {
            var cutOffStartDate = GetCutOffDateUsingDefault(dateRange.Start.ToMinimumHourValue(), isCutOffStart: true);
            var cutOffEndDate = GetCutOffDateUsingDefault(dateRange.End.ToMinimumHourValue(), isCutOffStart: false);

            return new TimePeriod(cutOffStartDate, cutOffEndDate);
        }

        private DateTime GetCutOffDateUsingDefault(DateTime date, bool isCutOffStart)
        {
            int month = date.Month;
            int year = date.Year;

            DaysSpan firstHalf = _policy.DefaultFirstHalfDaysSpan();
            DaysSpan endOfTheMonth = _policy.DefaultEndOfTheMonthDaysSpan();

            DaysSpan currentDaySpan = firstHalf.IsBetween(date) ? firstHalf : endOfTheMonth;

            return isCutOffStart ?
                currentDaySpan.From.GetDate(month: month, year: year) :
                currentDaySpan.To.GetDate(month: month, year: year);
        }

        #endregion Private helper methods
    }
}