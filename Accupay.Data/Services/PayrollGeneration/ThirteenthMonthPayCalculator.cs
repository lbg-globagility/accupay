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

        public ThirteenthMonthPay Calculate(Employee employee,
                            Paystub paystub,
                            IReadOnlyCollection<TimeEntry> timeEntries,
                            IReadOnlyCollection<ActualTimeEntry> actualTimeEntries,
                            Salary salary,
                            ListOfValueCollection settings,
                            ICollection<AllowanceItem> allowanceItems,
                            string currentSystemOwner)
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

            decimal thirteenthMonthBasicPay;

            var thirteenMonthCalculation = settings
                .GetEnum("ThirteenthMonthPolicy.CalculationBasis", ThirteenthMonthCalculationBasis.RegularPayAndAllowance);

            thirteenthMonthBasicPay = GetThirteenMonthBasicPay(
                paystub,
                thirteenMonthCalculation,
                employee,
                timeEntries,
                actualTimeEntries,
                salary,
                settings,
                allowanceItems.ToList(),
                currentSystemOwner);

            var thirteenthMonthAmount = thirteenthMonthBasicPay / CalendarConstant.MonthsInAYear;

            paystub.ThirteenthMonthPay.BasicPay = thirteenthMonthBasicPay;
            paystub.ThirteenthMonthPay.Amount = thirteenthMonthAmount;
            paystub.ThirteenthMonthPay.Paystub = paystub;

            return paystub.ThirteenthMonthPay;
        }

        private static decimal GetThirteenMonthBasicPay(
            Paystub paystub,
            ThirteenthMonthCalculationBasis thirteenMonthPolicy,
            Employee employee,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualtimeentries,
            Salary salary,
            ListOfValueCollection settings,
            IReadOnlyCollection<AllowanceItem> allowanceItems,
            string currentSystemOwner)
        {
            switch (thirteenMonthPolicy)
            {
                case ThirteenthMonthCalculationBasis.RegularPayAndAllowance:
                    return ComputeByRegularPayAndAllowance(employee, timeEntries, actualtimeentries, salary, settings, allowanceItems);

                case ThirteenthMonthCalculationBasis.DailyRate:
                    var hoursWorked = paystub.TotalWorkedHoursWithoutOvertimeAndLeave;

                    if (currentSystemOwner == SystemOwnerService.Benchmark && employee.IsPremiumInclusive)
                    {
                        hoursWorked = paystub.RegularHoursAndTotalRestDay;
                    }

                    return ComputeByDailyRate(employee, salary, hoursWorked);

                default:
                    return 0;
            }
        }

        private static decimal ComputeByDailyRate(Employee employee, Salary salary, decimal hoursWorked)
        {
            var daysWorked = hoursWorked / PayrollTools.WorkHoursPerDay;

            var dailyRate = PayrollTools.GetDailyRate(salary, employee);

            return AccuMath.CommercialRound(daysWorked * dailyRate);
        }

        private static decimal ComputeByRegularPayAndAllowance(
            Employee employee,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualtimeentries,
            Salary salary,
            ListOfValueCollection settings,
            IReadOnlyCollection<AllowanceItem> allowanceItems)
        {
            var thirteenthMonthAmount = 0M;

            if (employee.IsDaily)
            {
                thirteenthMonthAmount = ComputeByRegularPayAndAllowanceDaily(
                    employee,
                    timeEntries,
                    actualtimeentries);
            }
            else if (employee.IsMonthly || employee.IsFixed)
            {
                if (salary == null) return 0;

                var trueSalary = salary.TotalSalary;
                var basicPay = trueSalary / CalendarConstant.SemiMonthlyPayPeriodsPerMonth;

                var totalDeductions = actualtimeentries.Sum(t => t.LateDeduction + t.UndertimeDeduction + t.AbsentDeduction);

                decimal additionalAmount = 0;
                if ((settings.GetBoolean("ThirteenthMonthPolicy.IsAllowancePaid")))
                {
                    // all allowance
                    additionalAmount = allowanceItems.Sum(a => a.Amount);
                }
                else
                {
                    // allowance that the 13th month pay flag is checked
                    additionalAmount = allowanceItems.Where(a => a.IsThirteenthMonthPay).Sum(a => a.Amount);
                }

                thirteenthMonthAmount = ((basicPay + additionalAmount) - totalDeductions);
            }

            return thirteenthMonthAmount;
        }

        // turn this back to private when we can use this class without coupling it with paystub
        // this should only be a calculator class, not an update paystub class
        public static decimal ComputeByRegularPayAndAllowanceDaily(
            Employee employee,
            IReadOnlyCollection<TimeEntry> timeEntries,
            IReadOnlyCollection<ActualTimeEntry> actualtimeentries)
        {
            decimal thirteenthMonthAmount = 0M;

            var contractualEmployementStatuses = new string[] { "Contractual", "SERVICE CONTRACT" };

            if (contractualEmployementStatuses.Contains(employee.EmploymentStatus))
            {
                thirteenthMonthAmount = timeEntries
                    .Where(t => !t.IsRestDay)
                    .Sum(t => t.BasicDayPay + t.LeavePay);
            }
            else
            {
                foreach (var actualTimeEntry in actualtimeentries)
                {
                    var timeEntry = timeEntries
                        .Where(t => t.Date == actualTimeEntry.Date)
                        .FirstOrDefault();

                    if (timeEntry == null || timeEntry.IsRestDay)
                    {
                        continue;
                    }

                    thirteenthMonthAmount += actualTimeEntry.BasicDayPay + actualTimeEntry.LeavePay;
                }
            }

            return thirteenthMonthAmount;
        }
    }
}