using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayPeriodRepository : ISavableRepository<PayPeriod>
    {
        Task<ICollection<PayPeriod>> GetAllSemiMonthlyThatHasPaystubsAsync(int organizationId);

        Task<PayPeriod> GetAsync(int organizationId, DateTime date);

        Task<PayPeriod> GetAsync(int organizationId, DateTime startDate, DateTime endDate);

        Task<PayPeriod> GetAsync(int organizationId, int month, int year, bool isFirstHalf);

        ICollection<PayPeriod> GetByMonthYearAndPayPrequency(int organizationId, int month, int year, int payFrequencyId);

        Task<ICollection<PayPeriod>> GetByMonthYearAndPayPrequencyAsync(int organizationId, int month, int year, int payFrequencyId);

        Task<ICollection<PayPeriod>> GetByYearAndPayPrequencyAsync(int organizationId, int year, int payFrequencyId);

        Task<ICollection<PayPeriod>> GetClosedPayPeriodsAsync(int organizationId, TimePeriod dateRange = null);

        PayPeriod GetCurrentOpen(int organizationId);

        Task<PayPeriod> GetCurrentOpenAsync(int organizationId);

        Task<PayPeriod> GetCurrentPayPeriodAsync(int organizationId, int currentUserId);

        Task<DateTime?> GetFirstDayOfTheYear(int currentPayPeriodYear, int organizationId);

        Task<PayPeriod> GetFirstPayPeriodOfTheYear(int currentPayPeriodYear, int organizationId);

        Task<DateTime?> GetLastDayOfTheYear(int currentPayPeriodYear, int organizationId);

        Task<PayPeriod> GetLatestAsync(int organizationId);

        Task<ICollection<PayPeriod>> GetLoanRemainingPayPeriodsAsync(int organizationId, DateTime startDate, string frequencySchedule, int count);

        Task<ICollection<PayPeriod>> GetLoanRemainingPayPeriodsAsync(Loan loans);

        PayPeriod GetNextPayPeriod(int payPeriodId, int organizationId);

        Task<PayPeriod> GetOpenOrCurrentPayPeriodAsync(int organizationId, int currentUserId);

        Task<PayPeriod> GetOrCreateCurrentPayPeriodAsync(int organizationId, int currentUserId);

        Task<PaginatedList<PayPeriod>> GetPaginatedListAsync(PageOptions options, int organizationId, int? year = null, string searchTerm = "");

        Task<ICollection<PayPeriod>> GetPeriodsFromThisPeriodOnwardsAsync(PayPeriod currentPayPeriod);

        Task<ICollection<PayPeriod>> GetYearlyPayPeriodsAsync(int organizationId, int year, int currentUserId);

        Task<bool> HasClosedPayPeriodAfterDateAsync(int organizationId, DateTime date);
    }
}
