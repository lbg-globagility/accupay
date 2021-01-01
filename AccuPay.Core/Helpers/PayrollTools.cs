using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Helpers
{
    /// <summary>
    /// All the functions in here should be moved to an application or domain service.
    /// </summary>
    public class PayrollTools
    {
        public const int MonthsPerYear = 12;

        public const int WorkHoursPerDay = 8;

        public const int SemiMonthlyPayPeriodsPerMonth = 2;

        // TODO: Transfer this and PayFrequencyWeeklyId to PayFrequency entity
        public const int PayFrequencySemiMonthlyId = 1;

        public const int PayFrequencyWeeklyId = 4;

        // this is also used by the DateTimePickers of windows desktops (WPF, windows forms)
        public static readonly DateTime SqlServerMinimumDate = new DateTime(1753, 1, 1);

        private const int PotentialLastWorkDay = 7;

        public static decimal GetEmployeeMonthlyRate(Employee employee,
                                                    Salary salary,
                                                    bool isActual = false)
        {
            if (employee == null || salary == null)
                return 0;

            var basicSalary = isActual ? salary.BasicSalary + salary.AllowanceSalary : salary.BasicSalary;

            if (employee.IsMonthly || employee.IsFixed)
                return basicSalary;
            else if (employee.IsDaily)
                return basicSalary * GetWorkDaysPerMonth(employee.WorkDaysPerYear);

            return 0;
        }

        public static decimal GetWorkDaysPerMonth(decimal workDaysPerYear)
        {
            return workDaysPerYear / MonthsPerYear;
        }

        public static decimal GetDailyRate(decimal monthlyRate, decimal workDaysPerYear)
        {
            return monthlyRate / GetWorkDaysPerMonth(workDaysPerYear);
        }

        public static decimal GetDailyRate(Salary salary, Employee employee, bool isActual = false)
        {
            var dailyRate = 0M;

            if (salary == null)
                return 0;

            var basicSalary = isActual ? salary.BasicSalary + salary.AllowanceSalary :
                                        salary.BasicSalary;

            if (employee.IsDaily)
                dailyRate = basicSalary;
            else if (employee.IsMonthly || employee.IsFixed)
            {
                if (employee.WorkDaysPerYear == 0)
                    return 0;
                dailyRate = basicSalary / (employee.WorkDaysPerYear / 12);
            }

            return dailyRate;
        }

        public static decimal GetHourlyRateByMonthlyRate(decimal monthlyRate, decimal workDaysPerYear)
        {
            return monthlyRate / GetWorkDaysPerMonth(workDaysPerYear) / WorkHoursPerDay;
        }

        public static decimal GetHourlyRateByDailyRate(decimal dailyRate)
        {
            return dailyRate / WorkHoursPerDay;
        }

        public static decimal GetHourlyRateByDailyRate(Salary salary,
                                                        Employee employee,
                                                        bool isActual = false)
        {
            return GetDailyRate(salary, employee, isActual) / WorkHoursPerDay;
        }

        public static bool HasWorkedLastWorkingDay(DateTime currentDate,
                                                    IReadOnlyCollection<TimeEntry> currentTimeEntries,
                                                    CalendarCollection calendarCollection)
        {
            var lastPotentialEntry = currentDate.Date.AddDays(-PotentialLastWorkDay);

            var lastTimeEntries = currentTimeEntries.
                                    Where(t => lastPotentialEntry <= t.Date && t.Date <= currentDate.Date).
                                    OrderByDescending(t => t.Date).
                                    ToList();

            foreach (var lastTimeEntry in lastTimeEntries)
            {
                // If employee has no shift set for the day, it's not a working day.
                if (lastTimeEntry.HasShift == false)
                    continue;

                var totalDayPay = lastTimeEntry.GetTotalDayPay();

                if (lastTimeEntry.IsRestDay)
                {
                    if (totalDayPay > 0)
                        return true;

                    continue;
                }

                var payrateCalendar = calendarCollection.GetCalendar(lastTimeEntry.BranchID);
                var payrate = payrateCalendar.Find(lastTimeEntry.Date);
                if (payrate.IsHoliday)
                {
                    if (totalDayPay > 0)
                        return true;

                    continue;
                }

                return lastTimeEntry.RegularHours > 0 || lastTimeEntry.TotalLeaveHours > 0;
            }

            return false;
        }

        public static bool HasWorkAfterLegalHoliday(DateTime legalHolidayDate,
                                                    DateTime endOfCutOff,
                                                    IList<TimeEntry> currentTimeEntries,
                                                    CalendarCollection calendarCollection)
        {
            var lastPotentialEntry = legalHolidayDate.Date.AddDays(PotentialLastWorkDay);

            var postTimeEntries = currentTimeEntries.
                                    Where(t => legalHolidayDate.Date < t.Date && t.Date <= lastPotentialEntry).
                                    OrderBy(t => t.Date).
                                    ToList();

            foreach (var timeEntry in postTimeEntries)
            {
                if (timeEntry.HasShift == false)
                    continue;

                var totalDayPay = timeEntry.GetTotalDayPay();

                if (timeEntry.IsRestDay)
                {
                    if (totalDayPay > 0)
                        return true;

                    continue;
                }

                var payrateCalendar = calendarCollection.GetCalendar(timeEntry.BranchID);
                var payrate = payrateCalendar.Find(timeEntry.Date);
                if (payrate.IsHoliday)
                {
                    if (totalDayPay > 0)
                        return true;

                    continue;
                }

                return timeEntry.RegularHours > 0 || timeEntry.TotalLeaveHours > 0;
            }

            // If holiday exactly falls in ending date of cut-off, and no attendance 3days after it
            // will treat it that employee was present
            if (!postTimeEntries.Any() && endOfCutOff == legalHolidayDate)
                return true;

            return false;
        }

        // TODO: GetOrganizationAddress
        //public static string GetOrganizationAddress()
        //{
        //    string str_quer_address = string.Concat("SELECT CONCAT_WS(', '", ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)", ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)", ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)", ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)", ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)", ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)", ") `Result`", " FROM organization og", " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID", " WHERE og.RowID = ", orgztnID, ";");

        //    return Convert.ToString(new SQL(str_quer_address).GetFoundRow);
        //}

        public static DateTime GetPreviousCutoffDateForCheckingLastWorkingDay(DateTime currentCutOffStart)
        {
            // Used to be 3 days since the starting cut off can be a Monday
            // so to check the last working day you have to check up to last Friday
            // and that is 3 days since starting cut off

            // But sometimes, last Friday can be a holiday.
            // Or more specifically, the last days of the previous cut off are holidays
            // for example January 1, 2020. December 30 & 31 are holidays, December 28 & 29
            // are weekends. So last working days is December 27, 5 days since the starting cutoff
            // so the original 3 days value will not be enough.

            // I chose 7 days but this can be modified if there are scenarios that needs
            // more than 7 days to check the last working day.

            return currentCutOffStart.AddDays(-PotentialLastWorkDay);
        }
    }
}
