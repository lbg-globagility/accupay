using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Helpers
{
    public class PayrollTools
    {
        public const int MonthsPerYear = 12;

        public const int WorkHoursPerDay = 8;

        public const int DivisorToDailyRate = 8;

        public const int SemiMonthlyPayPeriodsPerMonth = 2;

        public const int PayFrequencySemiMonthlyId = 1;

        public const int PayFrequencyWeeklyId = 4;

        public static readonly DateTime MinimumMicrosoftDate = new DateTime(1753, 1, 1);

        private const int PotentialLastWorkDay = 7;

        public static decimal GetEmployeeMonthlyRate(IEmployee employee,
                                                    ISalary salary,
                                                    bool isActual = false)
        {
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

        public static decimal GetDailyRate(ISalary salary, IEmployee employee, bool isActual = false)
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

        public static decimal GetHourlyRateByDailyRate(ISalary salary,
                                                        IEmployee employee,
                                                        bool isActual = false)
        {
            return GetDailyRate(salary, employee, isActual) / WorkHoursPerDay;
        }

        public static bool HasWorkedLastWorkingDay(DateTime currentDate,
                                                    ICollection<ITimeEntry> currentTimeEntries,
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
                                                    IList<ITimeEntry> currentTimeEntries,
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

        internal static async Task<IPayPeriod> GetCurrentlyWorkedOnPayPeriodByCurrentYear(int organizationId, IEnumerable<IPayPeriod> payperiods = null)
        {
            return await Task.Run(() =>
            {
                // replace this with a policy
                // fourlinq can use this feature also
                // for clients that has the same attendance and payroll period
                var isBenchmarkOwner = ((new SystemOwnerService()).GetCurrentSystemOwner() ==
                                                                    SystemOwnerService.Benchmark);

                var currentDay = DateTime.Today.ToMinimumHourValue();

                using (var context = new PayrollContext())
                {
                    if (payperiods == null || payperiods.Count() == 0)
                        payperiods = context.PayPeriods.
                                        Where(p => p.OrganizationID == organizationId).
                                        Where(p => p.IsSemiMonthly);

                    if (isBenchmarkOwner)
                        return payperiods.
                                Where(p => currentDay >= p.PayFromDate && currentDay <= p.PayToDate).
                                LastOrDefault();
                    else
                        return payperiods.
                                Where(p => p.PayToDate < currentDay).
                                LastOrDefault();
                }
            });
        }

        public static async Task<Allowance> GetOrCreateEmployeeEcola(int employeeId,
                                                                        int organizationId,
                                                                        int userId,
                                                                        TimePeriod timePeriod,
                                                                        string allowanceFrequency = Allowance.FREQUENCY_SEMI_MONTHLY,
                                                                        decimal amount = 0)
        {
            var allowanceRepository = new AllowanceRepository();
            var productRepository = new ProductRepository();

            var ecolaAllowance = await allowanceRepository.GetEmployeeEcolaAsync(employeeId: employeeId,
                                                                            organizationId: organizationId,
                                                                            timePeriod: timePeriod);

            if (ecolaAllowance == null)
            {
                var ecolaProductId = (await productRepository.GetOrCreateAllowanceTypeAsync(ProductConstant.ECOLA,
                                                                                        organizationId,
                                                                                        userId))?.RowID;

                DateTime? effectiveEndDate = null;

                ecolaAllowance = new Allowance();
                ecolaAllowance.EmployeeID = employeeId;
                ecolaAllowance.ProductID = ecolaProductId;
                ecolaAllowance.AllowanceFrequency = allowanceFrequency;
                ecolaAllowance.EffectiveStartDate = timePeriod.Start;
                ecolaAllowance.EffectiveEndDate = effectiveEndDate;
                ecolaAllowance.Amount = amount;
                ecolaAllowance.CreatedBy = userId;
                ecolaAllowance.OrganizationID = organizationId;

                await allowanceRepository.SaveAsync(ecolaAllowance);

                ecolaAllowance = await allowanceRepository.GetEmployeeEcolaAsync(employeeId: employeeId,
                                                                            organizationId: organizationId,
                                                                            timePeriod: timePeriod);
            }

            return ecolaAllowance;
        }

        internal static async Task<PayPeriod> GetFirstPayPeriodOfTheYear(PayrollContext context,
                                                                        PayPeriod currentPayPeriod,
                                                                        int organizationId)
        {
            var currentPayPeriodYear = currentPayPeriod?.Year;

            if (currentPayPeriodYear == null)
                return null;

            if (context == null)
                context = new PayrollContext();

            return await context.PayPeriods.Where(p => p.OrganizationID == organizationId).
                                            Where(p => p.Year == currentPayPeriodYear).
                                            Where(p => p.IsSemiMonthly).
                                            Where(p => p.IsFirstPayPeriodOfTheYear).
                                            FirstOrDefaultAsync();
        }

        internal static async Task<DateTime?> GetFirstDayOfTheYear(PayrollContext context,
                                                                    PayPeriod currentPayPeriod,
                                                                    int organizationId)
        {
            var firstPayPeriodOfTheYear = await GetFirstPayPeriodOfTheYear(context,
                                                                            currentPayPeriod,
                                                                            organizationId);

            return firstPayPeriodOfTheYear?.PayFromDate;
        }

        // TODO: UpdateLoanSchedule
        //public static void UpdateLoanSchedule(int paypRowID, int organizationId, int userId)
        //{
        //    var param_array = new object[] { organizationId, paypRowID, userId };

        //    string strquery_recompute_13monthpay = "call recompute_thirteenthmonthpay(?organizid, ?payprowid, ?userrowid);"

        //    var n_ExecSQLProcedure = new SQL(strquery_recompute_13monthpay, param_array);
        //    n_ExecSQLProcedure.ExecuteQuery();
        //}

        public static bool CheckIfUsingUserLevel()
        {
            var settings = ListOfValueCollection.Create();

            return settings.GetBoolean("User Policy.UseUserLevel", false);
        }

        // TODO: DeletePaystub
        //public static void DeletePaystub(int employeeId, int payPeriodId)
        //{
        //    ExecuteQuery n_ExecuteQuery = new ExecuteQuery("SELECT RowID" + " FROM paystub" + " WHERE EmployeeID='" + employeeId + "'" + " AND OrganizationID='" + orgztnID + "'" + " AND PayPeriodID='" + payPeriodId + "'" + " LIMIT 1;");

        //    object paystubRowID = null;

        //    paystubRowID = n_ExecuteQuery.Result;

        //    if (paystubRowID != null)
        //        n_ExecuteQuery = new ExecuteQuery("CALL DEL_specificpaystub('" + paystubRowID.ToString() + "');");
        //}

        public static async Task<FunctionResult> ValidatePayPeriodAction(int? payPeriodId, int organizationId)
        {
            var systemOwner = new SystemOwnerService();

            if (systemOwner.GetCurrentSystemOwner() == SystemOwnerService.Benchmark)
            {
                // Add temporarily. Consult maam mely first as she is still testing the system with multiple pay periods
                return FunctionResult.Success();
            }

            using (PayrollContext context = new PayrollContext())
            {
                if (payPeriodId == null)
                {
                    return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
                }

                var payPeriod = await context.PayPeriods.
                                FirstOrDefaultAsync(p => p.RowID == payPeriodId);

                if (payPeriod == null)
                {
                    return FunctionResult.Failed("Pay period does not exists. Please refresh the form.");
                }

                var otherProcessingPayPeriod = await context.Paystubs.
                                                            Include(p => p.PayPeriod).
                                                            Where(p => p.PayPeriod.RowID != payPeriodId).
                                                            Where(p => p.PayPeriod.IsClosed == false).
                                                            Where(p => p.PayPeriod.OrganizationID == organizationId).
                                                            FirstOrDefaultAsync();

                if (payPeriod.IsClosed)
                {
                    return FunctionResult.Failed("The pay period you selected is already closed. Please reopen so you can alter the data for that pay period. If there are \"Processing\" pay periods, make sure to close them first.");
                }
                else if (!payPeriod.IsClosed && otherProcessingPayPeriod != null)
                {
                    return FunctionResult.Failed("There is currently a pay period with \"PROCESSING\" status. Please finish that pay period first then close it to process other open pay periods.");
                }
            }

            return FunctionResult.Success();
        }

        public static PayPeriod GetNextPayPeriod(int payPeriodId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                var currentPayPeriod = context.PayPeriods.FirstOrDefault(p => p.RowID == payPeriodId);

                if (currentPayPeriod == null)
                    return null;

                return context.PayPeriods.
                                Where(p => p.OrganizationID == currentPayPeriod.OrganizationID).
                                Where(p => p.PayFrequencyID == currentPayPeriod.PayFrequencyID).
                                Where(p => p.PayFromDate > currentPayPeriod.PayFromDate).
                                OrderBy(p => p.PayFromDate).
                                FirstOrDefault();
            }
        }

        // TODO: GetOrganizationAddress
        //public static string GetOrganizationAddress()
        //{
        //    string str_quer_address = string.Concat("SELECT CONCAT_WS(', '", ", IF(LENGTH(TRIM(ad.StreetAddress1)) = 0, NULL, ad.StreetAddress1)", ", IF(LENGTH(TRIM(ad.StreetAddress2)) = 0, NULL, ad.StreetAddress2)", ", IF(LENGTH(TRIM(ad.Barangay)) = 0, NULL, ad.Barangay)", ", IF(LENGTH(TRIM(ad.CityTown)) = 0, NULL, ad.CityTown)", ", IF(LENGTH(TRIM(ad.Country)) = 0, NULL, ad.Country)", ", IF(LENGTH(TRIM(ad.State)) = 0, NULL, ad.State)", ") `Result`", " FROM organization og", " LEFT JOIN address ad ON ad.RowID = og.PrimaryAddressID", " WHERE og.RowID = ", orgztnID, ";");

        //    return Convert.ToString(new SQL(str_quer_address).GetFoundRow);
        //}

        public static CalendarCollection GetCalendarCollection(DateTime threeDaysBeforeCutoff,
                                                                DateTime payDateTo,
                                                                PayRateCalculationBasis calculationBasis,
                                                                int organizationId)
        {
            using (var context = new PayrollContext())
            {
                var payrates = context.PayRates.
                                    Where(p => p.OrganizationID == organizationId).
                                    Where(p => threeDaysBeforeCutoff <= p.Date && p.Date <= payDateTo).
                                    ToList();
                if (calculationBasis == PayRateCalculationBasis.Branch)
                {
                    BranchRepository branchRepository = new BranchRepository();
                    var branches = branchRepository.GetAll();

                    var calendarDays = context.CalendarDays.
                                                Include(t => t.DayType).
                                                Where(t => threeDaysBeforeCutoff <= t.Date &&
                                                                t.Date <= payDateTo).
                                                ToList();

                    return new CalendarCollection(payrates, (ICollection<Branch>)branches, calendarDays);
                }
                else
                    return new CalendarCollection(payrates);
            }
        }

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