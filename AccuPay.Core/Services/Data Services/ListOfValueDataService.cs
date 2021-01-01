using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services.Policies;
using AccuPay.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class ListOfValueDataService : BaseSavableDataService<ListOfValue>
    {
        private readonly ListOfValueRepository _listOfValueRepository;

        public ListOfValueDataService(
            ListOfValueRepository listOfValueRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext payrollContext,
            IPolicyHelper policy) :

            base(listOfValueRepository,
                payPeriodRepository,
                payrollContext,
                policy,
                "List of value")
        {
            _listOfValueRepository = listOfValueRepository;
        }

        protected override async Task SanitizeEntity(ListOfValue entity, ListOfValue oldEntity, int currentlyLoggedInUserId)
        {
            await base.SanitizeEntity(entity, oldEntity, currentlyLoggedInUserId);

            entity.CreatedBy = currentlyLoggedInUserId;
            entity.LastUpdBy = currentlyLoggedInUserId;
        }

        public async Task CreateOrUpdateDefaultPayPeriod(
            int organizationId,
            int currentlyLoggedInUserId,
            TimePeriod firstHalf,
            TimePeriod endOfTheMonth)
        {
            ValidateDefaultPayPeriodData(firstHalf, endOfTheMonth);

            await SaveDefaultPayPeriodData(
                organizationId: organizationId,
                currentlyLoggedInUserId: currentlyLoggedInUserId,
                firstHalf: firstHalf,
                endOfTheMonth: endOfTheMonth);
        }

        public async Task SaveDefaultPayPeriodData(int organizationId, int currentlyLoggedInUserId, TimePeriod firstHalf, TimePeriod endOfTheMonth)
        {
            var currentFirstHalfPolicy = await _listOfValueRepository.GetPolicyAsync(
                PolicyHelper.PayPeriodPolicyType,
                PolicyHelper.DefaultFirstHalfDaysSpanPolicyLIC,
                organizationId);

            var currentEndOfTheMonthPolicy = await _listOfValueRepository.GetPolicyAsync(
                PolicyHelper.PayPeriodPolicyType,
                PolicyHelper.DefaultEndOfTheMonthDaysSpanPolicyLIC,
                organizationId);

            if (currentFirstHalfPolicy == null)
            {
                currentFirstHalfPolicy = ListOfValue.NewPolicy(
                    value: null,
                    lic: PolicyHelper.DefaultFirstHalfDaysSpanPolicyLIC,
                    type: PolicyHelper.PayPeriodPolicyType,
                    organizationId: organizationId
                );
            }

            if (currentEndOfTheMonthPolicy == null)
            {
                currentEndOfTheMonthPolicy = ListOfValue.NewPolicy(
                    value: null,
                    lic: PolicyHelper.DefaultEndOfTheMonthDaysSpanPolicyLIC,
                    type: PolicyHelper.PayPeriodPolicyType,
                    organizationId: organizationId
                );
            }

            currentFirstHalfPolicy.DisplayValue = new DefaultPayPeriod(firstHalf).ToString();
            currentEndOfTheMonthPolicy.DisplayValue = new DefaultPayPeriod(endOfTheMonth).ToString();

            await SaveManyAsync(new List<ListOfValue>()
            {
                currentFirstHalfPolicy,
                currentEndOfTheMonthPolicy
            },
            currentlyLoggedInUserId);
        }

        public void ValidateDefaultPayPeriodData(TimePeriod firstHalf, TimePeriod endOfTheMonth)
        {
            int decemberMonth = 12;
            int januaryMonth = 1;

            if (
                (firstHalf.Start.Month != decemberMonth && firstHalf.Start.Month != januaryMonth) ||
                (firstHalf.End.Month != decemberMonth && firstHalf.End.Month != januaryMonth) ||
                (endOfTheMonth.Start.Month != decemberMonth && endOfTheMonth.Start.Month != januaryMonth) ||
                (endOfTheMonth.End.Month != decemberMonth && endOfTheMonth.End.Month != januaryMonth)
                )
                throw new BusinessLogicException("First cut off should be at the earliest be on December on previous year and at the latest on January of the current year.");

            if (firstHalf.End.Date.AddDays(1) != endOfTheMonth.Start.Date)
                throw new BusinessLogicException("The day after the last day of first half cut off should be the first day of the end of the month cut off.");

            if (endOfTheMonth.End.Date.AddDays(1) != firstHalf.Start.Date.AddMonths(1))
                throw new BusinessLogicException("The day after the last day of end of the month cut off should be the first day of the first half cut off of the next month.");

            TimeSpan fifteenDays = new TimeSpan(15, 0, 0, 0, 0);
            TimeSpan sixteenDays = new TimeSpan(16, 0, 0, 0, 0);

            TimeSpan firstHalfDuration = firstHalf.End.Date.AddDays(1) - firstHalf.Start.Date;
            TimeSpan endOfTheMonthDuration = endOfTheMonth.End.Date.AddDays(1) - endOfTheMonth.Start.Date;

            if (firstHalfDuration < fifteenDays || firstHalfDuration > sixteenDays)
                throw new BusinessLogicException("First half cut off duration must be between 15 and 16 days.");

            if (endOfTheMonthDuration < fifteenDays || endOfTheMonthDuration > sixteenDays)
                throw new BusinessLogicException("End of the month cut off duration must be between 15 and 16 days.");
        }
    }
}
