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
        private readonly IPolicyHelper _policy;

        public PayPeriodRepository(PayrollContext context, IPolicyHelper policy) : base(context)
        {
            _policy = policy;
        }

        #region Queries

        #region Single entity

        /// <summary>
        /// Gets the latest payperiod that is Closed or Open. (Used in Web)
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

        /// <summary>
        /// Gets the current open pay period asyncrhonously.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetCurrentOpenAsync(int organizationId)
        {
            IQueryable<PayPeriod> query = CreateBaseQueryGetCurrentOpen(organizationId);

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the current open pay period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public PayPeriod GetCurrentOpen(int organizationId)
        {
            IQueryable<PayPeriod> query = CreateBaseQueryGetCurrentOpen(organizationId);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Gets the current Open pay period. If there is no Open pay period, get the current pay period based on current date.
        /// (This can return a dynamically generated pay period. It means that it can return a complete pay period without RowID)
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetOpenOrCurrentPayPeriodAsync(int organizationId, int currentUserId)
        {
            var payPeriod = await GetCurrentOpenAsync(organizationId);

            if (payPeriod == null)
            {
                payPeriod = await GetCurrentPayPeriodAsync(organizationId, currentUserId);
            }

            return payPeriod;
        }

        /// <summary>
        /// Gets the current pay period based on current date.
        /// (This can return a dynamically generated pay period. It means that it can return a complete pay period without RowID)
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetCurrentPayPeriodAsync(int organizationId, int currentUserId)
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

            var payPeriod = await query.FirstOrDefaultAsync();

            if (payPeriod == null)
            {
                payPeriod = PayPeriod.NewPayPeriod(
                    organizationId: organizationId,
                    payrollMonth: currentDay.Month,
                    payrollYear: currentDay.Year,
                    isFirstHalf: isFirstHalf,
                    policy: _policy,
                    currentUserId: currentUserId);
            }

            return payPeriod;
        }

        /// <summary>
        /// Gets the current pay period based on current date. If it does not exists, it creates a pay period based on current date.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetOrCreateCurrentPayPeriodAsync(int organizationId, int currentUserId)
        {
            var payPeriod = await GetCurrentPayPeriodAsync(organizationId, currentUserId);

            // if pay period was auto generated (does not exists in database),
            // save it to database
            if (payPeriod != null && payPeriod.RowID == null)
            {
                await SaveAsync(payPeriod);
            }

            return payPeriod;
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
                .Where(x => x.IsBetween(date))
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the pay period based on checking wether the passed organizationId, payFromDate, payToDate and it's half type matches the pay period.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetAsync(int organizationId, DateTime startDate, DateTime endDate)
        {
            return await CreateBaseQuery(organizationId)
                .Where(x => x.PayFromDate == startDate)
                .Where(x => x.PayToDate == endDate)
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

        /// <summary>
        /// Gets the next pay period of the passed pay period.
        /// (This can return a dynamically generated pay period. It means that it can return a complete pay period without RowID)
        /// </summary>
        /// <param name="payPeriodId">The passed pay period Id.</param>
        /// <param name="organizationId">The RowID of the organization.</param>
        /// <returns></returns>
        public PayPeriod GetNextPayPeriod(int payPeriodId, int organizationId)
        {
            var currentPayPeriod = _context.PayPeriods.FirstOrDefault(p => p.RowID == payPeriodId);

            if (currentPayPeriod == null)
                return null;

            var nextPayPeriod = _context.PayPeriods
                .Where(p => p.OrganizationID == currentPayPeriod.OrganizationID)
                .Where(p => p.PayFrequencyID == currentPayPeriod.PayFrequencyID)
                .Where(p => p.PayFromDate > currentPayPeriod.PayFromDate)
                .OrderBy(p => p.PayFromDate)
                .FirstOrDefault();

            if (nextPayPeriod == null)
            {
                var isFirstHalf = !currentPayPeriod.IsFirstHalf;

                int year = currentPayPeriod.Year;
                var month = currentPayPeriod.IsEndOfTheMonth ? currentPayPeriod.Month + 1 : currentPayPeriod.Month;

                if (currentPayPeriod.IsLastPayPeriodOfTheYear)
                {
                    year = currentPayPeriod.Year + 1;
                    month = PayPeriod.FirstPayrollMonth;
                }

                nextPayPeriod = PayPeriod.NewPayPeriod(
                    organizationId: organizationId,
                    payrollMonth: year,
                    payrollYear: month,
                    isFirstHalf: isFirstHalf,
                    policy: _policy,
                    currentUserId: null);
            }

            return nextPayPeriod;
        }

        /// <summary>
        /// Gets the first pay period of the year.
        /// (This can return a dynamically generated pay period. It means that it can return a complete pay period without RowID)
        /// </summary>
        /// <param name="currentPayPeriodYear"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public async Task<PayPeriod> GetFirstPayPeriodOfTheYear(int currentPayPeriodYear, int organizationId)
        {
            var firstPayPeriod = await CreateBaseQuery(organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsFirstPayPeriodOfTheYear)
                .FirstOrDefaultAsync();

            if (firstPayPeriod == null)
            {
                firstPayPeriod = PayPeriod.NewPayPeriod(
                    organizationId: organizationId,
                    payrollMonth: PayPeriod.FirstPayrollMonth,
                    payrollYear: currentPayPeriodYear,
                    isFirstHalf: true,
                    policy: _policy,
                    currentUserId: null);
            }

            return firstPayPeriod;
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

        public async Task<ICollection<PayPeriod>> GetYearlyPayPeriodsAsync(int organizationId, int year, int currentUserId)
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
                    firstHalf = PayPeriod.NewPayPeriod(
                        organizationId: organizationId,
                        payrollMonth: month,
                        payrollYear: year,
                        isFirstHalf: true,
                        policy: _policy,
                        currentUserId: currentUserId);
                }

                if (endOfTheMonth == null)
                {
                    endOfTheMonth = PayPeriod.NewPayPeriod(
                        organizationId: organizationId,
                        payrollMonth: month,
                        payrollYear: year,
                        isFirstHalf: false,
                        policy: _policy,
                        currentUserId: currentUserId);
                }

                yearlyPayPeriods.Add(firstHalf);
                yearlyPayPeriods.Add(endOfTheMonth);
            }

            return yearlyPayPeriods;
        }

        public async Task<PaginatedList<PayPeriod>> GetPaginatedListAsync(PageOptions options, int organizationId, int? year = null, string searchTerm = "")
        {
            var query = CreateBaseQuery(organizationId)
                .AsNoTracking()
                .Where(t => t.Status != PayPeriodStatus.Pending)
                .OrderByDescending(t => t.PayFromDate)
                .AsQueryable();

            if (year != null)
            {
                query = query.Where(x => x.Year == year);
            }

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

        public async Task<ICollection<PayPeriod>> GetClosedPayPeriodsAsync(int organizationId, TimePeriod dateRange = null)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(p => p.Status == PayPeriodStatus.Closed);

            if (dateRange != null)
            {
                var cutOff = GetCutOffPeriodUsingDefault(dateRange, organizationId);

                query = query
                    .Where(x => x.PayFromDate >= cutOff.Start)
                    .Where(x => x.PayToDate <= cutOff.End);
            }

            return await query.ToListAsync();
        }

        public async Task<ICollection<PayPeriod>> GetLoanScheduleRemainingPayPeriodsAsync(LoanSchedule loanSchedule)
        {
            int organizationId = loanSchedule.OrganizationID.Value;
            DateTime startDate = loanSchedule.DedEffectiveDateFrom;
            string frequencySchedule = loanSchedule.DeductionSchedule;
            int count = (int)loanSchedule.TotalPayPeriod;

            return await GetLoanScheduleRemainingPayPeriodsAsync(
                organizationId: organizationId,
                startDate: startDate,
                frequencySchedule: frequencySchedule,
                count: count);
        }

        public async Task<ICollection<PayPeriod>> GetLoanScheduleRemainingPayPeriodsAsync(
            int organizationId,
            DateTime startDate,
            string frequencySchedule,
            int count)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(pp => pp.PayFrequencyID == PayrollTools.PayFrequencySemiMonthlyId)
                .Where(pp => pp.PayToDate >= startDate);

            if (frequencySchedule == ContributionSchedule.FIRST_HALF)
            {
                query = query.Where(pp => pp.IsFirstHalf);
            }
            else if (frequencySchedule == ContributionSchedule.END_OF_THE_MONTH)
            {
                query = query.Where(pp => pp.IsEndOfTheMonth);
            }

            return await query
                .AsNoTracking()
                .OrderBy(pp => pp.Year)
                    .ThenBy(pp => pp.PayFromDate)
                .Take(count)
                .ToListAsync();
        }

        internal async Task<ICollection<PayPeriod>> GetPeriodsFromThisPeriodOnwardsAsync(PayPeriod currentPayPeriod)
        {
            var query = CreateBaseQuery(currentPayPeriod.OrganizationID.Value)
                .Include(pp => pp.Paystubs)
                    .ThenInclude(ps => ps.LoanTransactions)
                .Where(pp => pp.PayFromDate > currentPayPeriod.PayFromDate)
                .Where(pp => pp.Paystubs.Any());

            return await query.ToListAsync();
        }

        #endregion List of entities

        #region Others

        public async Task<DateTime?> GetFirstDayOfTheYear(int currentPayPeriodYear, int organizationId)
        {
            var firstPayPeriodOfTheYear = await GetFirstPayPeriodOfTheYear(
                currentPayPeriodYear: currentPayPeriodYear,
                organizationId: organizationId);

            return firstPayPeriodOfTheYear?.PayFromDate;
        }

        public async Task<DateTime?> GetLastDayOfTheYear(int currentPayPeriodYear, int organizationId)
        {
            var lastDayOfTheYear = await CreateBaseQuery(organizationId)
                .Where(p => p.Year == currentPayPeriodYear)
                .Where(p => p.IsLastPayPeriodOfTheYear)
                .Select(p => p.PayToDate)
                .FirstOrDefaultAsync();

            if (lastDayOfTheYear == null)
            {
                var lastPayPeriodOfTheYear = PayPeriod.NewPayPeriod(
                    organizationId: organizationId,
                    payrollMonth: PayPeriod.LastPayrollMonth,
                    payrollYear: currentPayPeriodYear,
                    isFirstHalf: false,
                    policy: _policy,
                    currentUserId: null);

                lastDayOfTheYear = lastPayPeriodOfTheYear.PayToDate;
            }

            return lastDayOfTheYear;
        }

        /// <summary>
        /// Check if there is a closed pay period after the date.
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<bool> HasClosedPayPeriodAfterDateAsync(int organizationId, DateTime date)
        {
            var query = CreateBaseQuery(organizationId)
                .Where(p => p.Status == PayPeriodStatus.Closed);

            var cutOffStart = GetCutOffPeriodUsingDefault(
                    new TimePeriod(date, date),
                    organizationId)
                .Start;
            query = query.Where(x => x.PayFromDate >= cutOffStart);

            return await query.AnyAsync();
        }

        #endregion Others

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
                .AsNoTracking()
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

        private TimePeriod GetCutOffPeriodUsingDefault(TimePeriod dateRange, int organizationId)
        {
            var cutOffStartDate = GetCutOffDateUsingDefault(
                dateRange.Start.ToMinimumHourValue(),
                isCutOffStart: true,
                organizationId: organizationId);

            var cutOffEndDate = GetCutOffDateUsingDefault(
                dateRange.End.ToMinimumHourValue(),
                isCutOffStart: false,
                organizationId: organizationId);

            return new TimePeriod(cutOffStartDate, cutOffEndDate);
        }

        private DateTime GetCutOffDateUsingDefault(DateTime date, bool isCutOffStart, int organizationId)
        {
            DayValueSpan firstHalf = _policy.DefaultFirstHalfDaysSpan(organizationId);
            DayValueSpan endOfTheMonth = _policy.DefaultEndOfTheMonthDaysSpan(organizationId);

            (DayValueSpan currentDaySpan, int month, int year) = PayPeriodHelper.GetCutOffDayValueSpan(date, firstHalf, endOfTheMonth);

            return isCutOffStart ?
                currentDaySpan.From.GetDate(month: month, year: year) :
                currentDaySpan.To.GetDate(month: month, year: year);
        }

        #endregion Private helper methods
    }
}
