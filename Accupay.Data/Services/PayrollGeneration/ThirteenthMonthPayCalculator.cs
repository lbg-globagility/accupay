using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class ThirteenthMonthPayCalculator
    {
        private readonly int _organizationId;
        private readonly int _userId;

        public ThirteenthMonthPayCalculator(int organizationId, int userId)
        {
            _organizationId = organizationId;
            _userId = userId;
        }

        public void Calculate(Employee employee,
                            Paystub paystub,
                            ICollection<TimeEntry> timeEntries,
                            ICollection<ActualTimeEntry> actualtimeentries,
                            Salary salary,
                            ListOfValueCollection settings,
                            ICollection<AllowanceItem> allowanceItems)
        {
            if (paystub.ThirteenthMonthPay == null)
                paystub.ThirteenthMonthPay = new ThirteenthMonthPay()
                {
                    OrganizationID = _organizationId,
                    CreatedBy = _userId,
                    Created = DateTime.Now
                };
            else
                paystub.ThirteenthMonthPay.LastUpdBy = _userId;

            decimal thirteenthMonthAmount;

            var thirteenMonthCalculation = settings.GetEnum("ThirteenthMonthPolicy.CalculationBasis", ThirteenthMonthCalculationBasis.RegularPayAndAllowance);

            thirteenthMonthAmount = GetThirteenMonthAmount(paystub, thirteenMonthCalculation, employee, timeEntries, actualtimeentries, salary, settings, allowanceItems);

            paystub.ThirteenthMonthPay.BasicPay = thirteenthMonthAmount;
            paystub.ThirteenthMonthPay.Amount = thirteenthMonthAmount / CalendarConstants.MonthsInAYear;
            paystub.ThirteenthMonthPay.Paystub = paystub;
        }

        private static decimal GetThirteenMonthAmount(Paystub paystub, ThirteenthMonthCalculationBasis thirteenMonthPolicy, Employee employee, ICollection<TimeEntry> timeEntries, ICollection<ActualTimeEntry> actualtimeentries, Salary salary, ListOfValueCollection settings, ICollection<AllowanceItem> allowanceItems)
        {
            switch (thirteenMonthPolicy)
            {
                case ThirteenthMonthCalculationBasis.RegularPayAndAllowance:
                    return ComputeRegularPayAndAllowance(employee, timeEntries, actualtimeentries, salary, settings, allowanceItems);

                case ThirteenthMonthCalculationBasis.DailyRate:
                    var hoursWorked = paystub.TotalWorkedHoursWithoutOvertimeAndLeave;

                    if ((new SystemOwnerService()).GetCurrentSystemOwner() == SystemOwnerService.Benchmark && employee.IsPremiumInclusive)
                        hoursWorked = paystub.RegularHoursAndTotalRestDay;

                    var daysWorked = hoursWorked / PayrollTools.WorkHoursPerDay;

                    var dailyRate = PayrollTools.GetDailyRate(salary, employee);

                    return AccuMath.CommercialRound(daysWorked * dailyRate);

                default:
                    return 0;
            }
        }

        private static decimal ComputeRegularPayAndAllowance(Employee _employee, ICollection<TimeEntry> timeEntries, ICollection<ActualTimeEntry> actualtimeentries, Salary salary, ListOfValueCollection settings, ICollection<AllowanceItem> allowanceItems)
        {
            var contractualEmployementStatuses = new string[] { "Contractual", "SERVICE CONTRACT" };

            var thirteenthMonthAmount = 0M;

            if (_employee.IsDaily)
            {
                if (contractualEmployementStatuses.Contains(_employee.EmploymentStatus))
                    thirteenthMonthAmount = timeEntries.
                                                Where(t => !t.IsRestDay).
                                                Sum(t => t.BasicDayPay + t.LeavePay);
                else
                {
                    foreach (var actualTimeEntry in actualtimeentries)
                    {
                        var timeEntry = timeEntries.
                                        Where(t => t.Date == actualTimeEntry.Date).
                                        FirstOrDefault();

                        if (timeEntry == null || timeEntry.IsRestDay)
                            continue;

                        thirteenthMonthAmount += actualTimeEntry.BasicDayPay + actualTimeEntry.LeavePay;
                    }
                }
            }
            else if (_employee.IsMonthly || _employee.IsFixed)
            {
                var trueSalary = salary.TotalSalary;
                var basicPay = trueSalary / CalendarConstants.SemiMonthlyPayPeriodsPerMonth;

                var totalDeductions = actualtimeentries.Sum(t => t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction);

                decimal additionalAmount = 0;
                if ((settings.GetBoolean("ThirteenthMonthPolicy.IsAllowancePaid")))
                    additionalAmount = allowanceItems.Sum(a => a.Amount);

                thirteenthMonthAmount = ((basicPay + additionalAmount) - totalDeductions);
            }

            var allowanceAmount = allowanceItems.Where(a => a.IsThirteenthMonthPay).Sum(a => a.Amount);
            thirteenthMonthAmount += allowanceAmount;
            return thirteenthMonthAmount;
        }
    }
}