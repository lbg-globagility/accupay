using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;

namespace AccuPay.Core.Services
{
    public class SemiMonthlyAllowanceCalculator
    {
        private readonly Employee _employee;
        private readonly Paystub _paystub;
        private readonly PayPeriod _payperiod;
        private readonly IReadOnlyCollection<TimeEntry> _timeEntries;

        private readonly AllowancePolicy _allowancePolicy;
        private readonly CalendarCollection _calendarCollection;

        private readonly int _userId;

        public SemiMonthlyAllowanceCalculator(
            AllowancePolicy allowancePolicy,
            Employee employee,
            Paystub paystub,
            PayPeriod payperiod,
            CalendarCollection calendarCollection,
            IReadOnlyCollection<TimeEntry> timeEntries,
            int currentlyLoggedInUserId)
        {
            _employee = employee;
            _paystub = paystub;
            _payperiod = payperiod;
            _calendarCollection = calendarCollection;
            _timeEntries = timeEntries;
            _userId = currentlyLoggedInUserId;
            _allowancePolicy = allowancePolicy;
        }

        public AllowanceItem Calculate(Allowance allowance)
        {
            var allowanceItem = AllowanceItem.Create(
                paystub: _paystub,
                product: allowance.Product,
                payperiodId: _payperiod.RowID.Value,
                allowanceId: allowance.RowID.Value,
                currentlyLoggedInUserId: _userId);

            allowanceItem.Amount = allowance.Amount;

            if (allowance.Product.Fixed)
                return allowanceItem;

            var monthlyRate = allowance.Amount * 2;
            var hourlyRate = PayrollTools
                .GetHourlyRateByMonthlyRate(monthlyRate, _employee.WorkDaysPerYear);

            foreach (var timeEntry in _timeEntries)
            {
                var deductionHours = timeEntry.LateHours + timeEntry.UndertimeHours + timeEntry.AbsentHours;
                var deductionAmount = -(hourlyRate * deductionHours);

                var additionalAmount = 0M;

                var payrateCalendar = _calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);

                if (_allowancePolicy.IsSpecialHolidayPaid)
                {
                    if ((payrate.IsSpecialNonWorkingHoliday && _employee.CalcSpecialHoliday))
                    {
                        additionalAmount =
                            timeEntry.SpecialHolidayHours *
                            hourlyRate *
                            (payrate.RegularRate - 1M);
                    }
                }

                if (_allowancePolicy.IsRegularHolidayPaid)
                {
                    if ((payrate.IsRegularHoliday && _employee.CalcHoliday))
                    {
                        additionalAmount =
                            timeEntry.RegularHolidayHours *
                            hourlyRate *
                            (payrate.RegularRate - 1M);
                    }
                }

                allowanceItem.AddPerDay(timeEntry.Date, deductionAmount + additionalAmount);
            }

            return allowanceItem;
        }
    }
}
