using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class DailyAllowanceCalculator
    {
        private ListOfValueCollection _settings;

        private IReadOnlyCollection<TimeEntry> _previousTimeEntries;
        private readonly CalendarCollection _calendarCollection;

        private readonly int _organizationId;

        private readonly int _userId;

        public DailyAllowanceCalculator(ListOfValueCollection settings,
                                        CalendarCollection calendarCollection,
                                        IReadOnlyCollection<TimeEntry> previousTimeEntries,
                                        int organizationId,
                                        int userId)
        {
            _settings = settings;
            _calendarCollection = calendarCollection;
            _previousTimeEntries = previousTimeEntries;
            _organizationId = organizationId;
            _userId = userId;
        }

        public AllowanceItem Compute(PayPeriod payperiod,
                                    Allowance allowance,
                                    Employee employee,
                                    Paystub paystub,
                                    IReadOnlyCollection<TimeEntry> timeEntries)
        {
            var dailyRate = allowance.Amount;

            if (payperiod.RowID == null || allowance.RowID == null) return null;

            var allowanceItem = AllowanceItem.Create(paystub: paystub,
                                                    product: allowance.Product,
                                                    payperiodId: payperiod.RowID.Value,
                                                    allowanceId: allowance.RowID.Value,
                                                    organizationId: _organizationId,
                                                    userId: _userId);

            foreach (var timeEntry in timeEntries)
            {
                if (!(allowance.EffectiveStartDate <= timeEntry.Date &&
                            (allowance.EffectiveEndDate == null ||
                            timeEntry.Date <= allowance.EffectiveEndDate)))
                    continue;

                var hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate);

                var allowanceAmount = 0M;
                var payrateCalendar = _calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);

                if (payrate.IsRegularDay)
                {
                    var isRestDay = timeEntry.RestDayHours > 0;

                    if (isRestDay)
                        allowanceAmount = dailyRate;
                    else if (allowance.Product.Fixed)
                        allowanceAmount = dailyRate;
                    else
                        allowanceAmount = (timeEntry.RegularHours + timeEntry.TotalLeaveHours) * hourlyRate;
                }
                else if (payrate.IsSpecialNonWorkingHoliday)
                {
                    var countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours;

                    allowanceAmount = countableHours > 0 ? dailyRate : 0M;
                }
                else if (payrate.IsRegularHoliday)
                {
                    allowanceAmount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate;

                    var exemption = _settings.GetBoolean("AllowancePolicy.HolidayAllowanceForMonthly");

                    var givePremium = _settings.GetBoolean("AllowancePolicy.NoPremium") == false;
                    var giveAdditionalHolidayPay = givePremium &&
                                                (((employee.IsFixed || employee.IsMonthly) && exemption) ||
                                                    PayrollTools.HasWorkedLastWorkingDay(timeEntry.Date,
                                                                                        _previousTimeEntries,
                                                                                        _calendarCollection));

                    if (giveAdditionalHolidayPay)
                    {
                        decimal basicHolidayPay;

                        if (_settings.GetString("AllowancePolicy.CalculationType") == "Hourly")
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