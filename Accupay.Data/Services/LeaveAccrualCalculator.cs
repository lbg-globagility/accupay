using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System;

namespace AccuPay.Data.Services
{
    public class LeaveAccrualCalculator
    {
        public decimal Calculate2(Employee employee,
                                  DateTime currentDate,
                                  decimal leaveAllowance,
                                  LeaveTransaction lastTransaction)
        {
            DateTime nextDate;
            if (lastTransaction == null)
                nextDate = employee.StartDate.AddMonths(1).AddDays(1);
            else
                nextDate = lastTransaction.TransactionDate.AddMonths(1);

            // Check if the current date is still too early to update the ledger
            if (currentDate < nextDate)
                return 0M;

            var leaveHoursPerMonth = leaveAllowance / (decimal)CalendarConstants.MonthsInAYear;

            var lastTransactionDate = lastTransaction?.TransactionDate ?? employee.StartDate;

            var previousTotalMonths = Calendar.MonthsBetween(employee.StartDate, lastTransactionDate);
            var previousTotal = AccuMath.CommercialRound(leaveHoursPerMonth * previousTotalMonths);

            var currentTotalMonths = Calendar.MonthsBetween(employee.StartDate, nextDate);
            var currentTotal = AccuMath.CommercialRound(leaveHoursPerMonth * currentTotalMonths);

            var leaveHours = currentTotal - previousTotal;

            return leaveHours;
        }

        public decimal Calculate(Employee employee,
                                 PayPeriod currentPayperiod,
                                 decimal leaveAllowance,
                                 PayPeriod firstPayperiodOfYear,
                                 PayPeriod lastPayperiodOfYear)
        {
            var daysInYear = Calendar.DaysBetween(firstPayperiodOfYear.PayFromDate, lastPayperiodOfYear.PayToDate);

            if (currentPayperiod.PayFromDate < employee.StartDate)
            {
                var daysInCutoff = Calendar.DaysBetween(employee.StartDate, currentPayperiod.PayToDate);
                var leaveHours = ComputeTotalLeave(daysInCutoff, daysInYear, leaveAllowance);

                return leaveHours;
            }

            if (currentPayperiod.RowID == firstPayperiodOfYear.RowID)
            {
                var daysInCutoff = Calendar.DaysBetween(currentPayperiod.PayFromDate, currentPayperiod.PayToDate);
                var leaveHours = ComputeTotalLeave(daysInCutoff, daysInYear, leaveAllowance);

                return leaveHours;
            }
            else if (currentPayperiod.RowID == lastPayperiodOfYear.RowID)
            {
                var daysInCutoff = Calendar.DaysBetween(currentPayperiod.PayFromDate, currentPayperiod.PayToDate);
                var leaveHours = ComputeTotalLeave(daysInCutoff, daysInYear, leaveAllowance);

                return leaveHours;
            }
            else
            {
                var startDateOfYear = firstPayperiodOfYear.PayFromDate;

                var daysSinceLastCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayFromDate.AddDays(-1));
                var totalSinceLastCutoff = ComputeTotalLeave(daysSinceLastCutoff, daysInYear, leaveAllowance);

                var daysSinceCurrentCutoff = Calendar.DaysBetween(startDateOfYear, currentPayperiod.PayToDate);
                var totalSinceCurrentCutoff = ComputeTotalLeave(daysSinceCurrentCutoff, daysInYear, leaveAllowance);

                // The difference between the total of the last cutoff and the total for the current cutoff will be the
                // actual leave hours to be given.
                var leaveHours = totalSinceCurrentCutoff - totalSinceLastCutoff;

                return leaveHours;
            }
        }

        private decimal ComputeTotalLeave(int daysPassed, int daysInYear, decimal leaveAllowance)
        {
            var leaves = decimal.Divide(daysPassed, daysInYear) * leaveAllowance;
            //var leaves = (daysPassed / (double)daysInYear) * leaveAllowance;

            return AccuMath.CommercialRound(leaves);
        }
    }
}
