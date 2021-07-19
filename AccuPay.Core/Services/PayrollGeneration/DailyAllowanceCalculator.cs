using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class DailyAllowanceCalculator
    {
        private readonly Employee _employee;
        private readonly Paystub _paystub;
        private readonly PayPeriod _payperiod;
        private readonly IReadOnlyCollection<TimeEntry> _previousTimeEntries;
        private readonly IReadOnlyCollection<Shift> _shifts;
        private readonly IReadOnlyCollection<TimeEntry> _timeEntries;

        private readonly AllowancePolicy _allowancePolicy;
        private readonly CalendarCollection _calendarCollection;
        private readonly int _userId;

        public DailyAllowanceCalculator(
            AllowancePolicy allowancePolicy,
            Employee employee,
            Paystub paystub,
            PayPeriod payperiod,
            CalendarCollection calendarCollection,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<TimeEntry> previousTimeEntries,
            int currentlyLoggedInUserId,
            IReadOnlyCollection<Shift> shifts = null)
        {
            _allowancePolicy = allowancePolicy;
            _employee = employee;
            _paystub = paystub;
            _payperiod = payperiod;
            _calendarCollection = calendarCollection;
            _timeEntries = timeEntries;
            _previousTimeEntries = previousTimeEntries;
            _shifts = shifts;
            _userId = currentlyLoggedInUserId;
        }

        public AllowanceItem Compute(Allowance allowance)
        {
            var dailyRate = allowance.Amount;

            if (_payperiod.RowID == null || allowance.RowID == null) return null;

            var allowanceItem = AllowanceItem.Create(
                paystub: _paystub,
                product: allowance.Product,
                payperiodId: _payperiod.RowID.Value,
                allowanceId: allowance.RowID.Value,
                currentlyLoggedInUserId: _userId);

            foreach (var timeEntry in _timeEntries)
            {
                if (!(allowance.EffectiveStartDate <= timeEntry.Date &&
                    (allowance.EffectiveEndDate == null ||
                    timeEntry.Date <= allowance.EffectiveEndDate)))
                {
                    continue;
                }

                var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate);

                var allowanceAmount = 0M;
                var payrateCalendar = _calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);
                var shift = _shifts.
                    Where(s => s.DateSched == timeEntry.Date).
                    FirstOrDefault();
                var markedAsWholeDay = shift?.MarkedAsWholeDay ?? false;

                if (payrate.IsRegularDay)
                {
                    var isRestDay = timeEntry.RestDayHours > 0;

                    if (isRestDay)
                        allowanceAmount = dailyRate;
                    else if (allowance.Product.Fixed)
                        allowanceAmount = dailyRate;
                    else if (markedAsWholeDay)
                    {
                        if (timeEntry.RegularHours > 0)
                            allowanceAmount =
                                timeEntry.BasicHours == shift.WorkHours ?
                                dailyRate :
                                dailyRate - ((shift.WorkHours - timeEntry.RegularHours) * hourlyRate);
                    }
                    else
                    {
                        var allowanceWorkedHours = _allowancePolicy.IsLeavePaid ?
                            timeEntry.RegularHours + timeEntry.TotalLeaveHours :
                            timeEntry.RegularHours;
                        allowanceAmount = allowanceWorkedHours * hourlyRate;
                    }
                }
                else if (payrate.IsSpecialNonWorkingHoliday)
                {
                    var countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours;

                    allowanceAmount = countableHours > 0 ? dailyRate : 0M;
                }
                else if (payrate.IsRegularHoliday)
                {
                    allowanceAmount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate;

                    var giveAdditionalHolidayPay =
                        !_allowancePolicy.NoPremium &&
                        (((_employee.IsFixed || _employee.IsMonthly) && _allowancePolicy.HolidayAllowanceForMonthly) ||
                            PayrollTools.HasWorkedLastWorkingDay(
                                timeEntry.Date,
                                _previousTimeEntries,
                                _calendarCollection));

                    if (giveAdditionalHolidayPay)
                    {
                        decimal basicHolidayPay;

                        if (_allowancePolicy.CalculationType == "Hourly")
                        {
                            var workHours = timeEntry.HasShift ? timeEntry.WorkHours : PayrollTools.WorkHoursPerDay;

                            basicHolidayPay = new decimal[] { workHours * hourlyRate, dailyRate }.Max();
                        }
                        else
                            basicHolidayPay = dailyRate;

                        if (payrate.IsDoubleHoliday)
                            // If it's a double holiday, then give the basic holiday pay twice
                            allowanceAmount += basicHolidayPay * 2;
                        else
                            allowanceAmount += basicHolidayPay;
                    }
                }

                allowanceItem.AddPerDay(timeEntry.Date, allowanceAmount);
            }

            return allowanceItem;
        }
    }
}
