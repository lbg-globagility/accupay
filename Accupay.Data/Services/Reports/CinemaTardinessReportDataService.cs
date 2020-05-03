using AccuPay.Data.Entities;
using AccuPay.Data.ReportModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class CinemaTardinessReportDataService
    {
        private const int JanuaryMonth = 1;
        private const int DecemberMonth = 12;

        private readonly int _organizationId;
        private readonly DateTime _selectedDate;
        private readonly bool _isLimitedReport;

        public CinemaTardinessReportDataService(int organizationId, DateTime selectedDate, bool isLimitedReport)
        {
            _organizationId = organizationId;
            _selectedDate = selectedDate;
            _isLimitedReport = isLimitedReport;
        }

        public async Task<List<ICinemaTardinessReportModel>> GetData()
        {
            var firstDayOfTheMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, 1);
            var lastDayOfTheMonth = new DateTime(_selectedDate.Year, _selectedDate.Month, DateTime.DaysInMonth(_selectedDate.Year, _selectedDate.Month));

            int previousMonth = _selectedDate.Month - 1;

            if (firstDayOfTheMonth.Month == JanuaryMonth)
                // if the firstDayOfTheMonth is 01/01/2020, previousMonth should be December (12)
                previousMonth = DecemberMonth;
            else
                // if the firstDayOfTheMonth is 02/01/2020, previousMonth should be January (1)
                previousMonth = _selectedDate.Month - 1;

            DateTime firstDayOneYear;
            if (firstDayOfTheMonth.Month == DecemberMonth)
                // if the firstDayOfTheMonth is 12/01/2019, lastYearDate is 1/01/2019
                firstDayOneYear = new DateTime(firstDayOfTheMonth.Year, JanuaryMonth, 1);
            else
                // if the firstDayOfTheMonth is 09/01/2019, lastYearDate is 10/01/2018
                firstDayOneYear = new DateTime(firstDayOfTheMonth.Year - 1, firstDayOfTheMonth.Month + 1, 1);

            var tardinessReportModels = new List<CinemaTardinessReportModel>();

            // #1. Get all the employee that have lates on the selected month
            // #2. If the affected employee has no record on the database for tardiness (for this year) yet, create a tardiness record for the employee
            // #3. Get all the tardiness record of the affected employees
            // #4. Update the value of [NumberOfOffense] from the data retrieved from #3

            var daysRequirement = _isLimitedReport ? CinemaTardinessReportModel.DaysLateLimit : 0;

            using (PayrollContext context = new PayrollContext())
            {
                // #1.
                var tardinessReportModelQuery = context.TimeEntries.
                                        Include(t => t.Employee).
                                        Where(t => t.Date >= firstDayOfTheMonth).
                                        Where(t => t.Date <= lastDayOfTheMonth).
                                        Where(t => t.OrganizationID == _organizationId).
                                        Where(t => t.LateHours > 0).
                                        GroupBy(t => t.Employee).
                                        Select(gt => new CinemaTardinessReportModel()
                                        {
                                            EmployeeId = gt.Key.RowID.Value,
                                            EmployeeName = gt.Key.FullNameWithMiddleInitialLastNameFirst,
                                            Days = gt.Count(),
                                            Hours = gt.Sum(t => t.LateHours)
                                        });

                if (_isLimitedReport)
                    tardinessReportModelQuery = tardinessReportModelQuery.
                                            Where(t => t.Days >= CinemaTardinessReportModel.DaysLateLimit);

                tardinessReportModels = await tardinessReportModelQuery.ToListAsync();

                // #2
                // this list contains the first offense dates per employee within the year of the report
                var employeeTardinessDates = await GetEmployeeTardinessRecordList(firstDayOfTheMonth, lastDayOfTheMonth, firstDayOneYear, tardinessReportModels, context);

                if (tardinessReportModels.Count != employeeTardinessDates.Count)
                    throw new Exception("Error creating new tardiness records.");

                // #3
                var earliestFirstOffenseDate = employeeTardinessDates.OrderBy(t => t.FirstOffenseDate).FirstOrDefault()?.FirstOffenseDate;

                if (earliestFirstOffenseDate == null)
                    earliestFirstOffenseDate = firstDayOfTheMonth;

                var previousOffenseList = await context.TimeEntries.
                                                    Include(t => t.Employee).
                                                    Where(t => t.Date >= earliestFirstOffenseDate.Value).
                                                    Where(t => t.Date <= lastDayOfTheMonth).
                                                    Where(t => t.OrganizationID == _organizationId).
                                                    Where(t => t.LateHours > 0).
                                                    ToListAsync();

                // #4
                foreach (var tardinessReportModel in tardinessReportModels)
                {
                    tardinessReportModel.NumberOfOffense = 0;

                    var firstOffenseDate = employeeTardinessDates.Where(e => e.EmployeeId == tardinessReportModel.EmployeeId).FirstOrDefault()?.FirstOffenseDate;

                    if (firstOffenseDate == null)
                        throw new Exception("Error creating new tardiness records.");

                    var employeeOffenseList = previousOffenseList.
                                            Where(o => o.EmployeeID == tardinessReportModel.EmployeeId).
                                            Where(o => o.Date >= firstOffenseDate).
                                            Where(o => o.Date <= lastDayOfTheMonth).
                                            ToList();

                    var employeeOffenseListPerMonth = employeeOffenseList.
                                                GroupBy(o => o.Date.Month).
                                                Select(m => new CinemaTardinessReportModel.PerMonth()
                                                {
                                                    EmployeeId = tardinessReportModel.EmployeeId,
                                                    Month = m.Key,
                                                    Days = m.Count(),
                                                    Hours = m.Sum(t => t.LateHours)
                                                }).ToList();

                    var previousNumberOfOffense = employeeOffenseListPerMonth.
                                        Where(o => o.Days >= CinemaTardinessReportModel.DaysLateLimit).
                                        Count();

                    tardinessReportModel.NumberOfOffense += previousNumberOfOffense;
                }

                await context.SaveChangesAsync();
            }

            // tardinessReportModels = GetSampleTardinessReportModels()

            return new List<ICinemaTardinessReportModel>(tardinessReportModels);
        }

        #region Private methods

        private async Task<List<EmployeeTardinessDate>> GetEmployeeTardinessRecordList(DateTime firstDayOfTheMonth, DateTime lastDayOfTheMonth, DateTime firstDayOneYear, List<CinemaTardinessReportModel> tardinessReportModels, PayrollContext context)
        {
            var employeeIds = tardinessReportModels.
                                Select(t => t.EmployeeId).
                                ToArray();
            // Original span [10/01/2018 to 09/30/2019]
            // Get the records that is within 2 years from now
            // ex. 11/01/2017 - 09/30/2019
            // because for example if there is a first offense tardiness record for Jan. 2018
            // That should only reset on Jan. 2019
            // That employee's tardiness span would be from Jan. 2019 to Sep. 2019
            // So the offenses that the employee got from Oct. 2018 to Dec. 2018 should not be included in this report
            // Those offenses will be included on the span of Jan. 2018 to Dec. 2018

            // Example 2
            // --
            // Jan 2018 to Dec 2018
            // Dec 2017 to Nov 2018
            // Nov 2017 to Oct 2018
            // -------------------------
            // Oct 2017 to Sept 2018
            // --

            // The [Nov. 2017 to Oct. 2018] timespan can still affect our September 2019 report
            // since if there is a first offense tardiness record on November 2017
            // an offense recorded on October 2018 would fit in that time span
            // that employees tardiness record would reset on November 2018
            // so the September 2019 report should not count that October 2018 offense

            var firstDayTwoYearsBefore = firstDayOfTheMonth.AddMonths(-22); // -22 because -2years which is -24 months +2months (Nov17-Sep19 span)

            await context.TardinessRecords.LoadAsync();

            var employeeTardinessRecords = context.TardinessRecords.Local.
                                                        Where(t => employeeIds.
                                                        Contains(t.EmployeeId)).
                                                        Where(t => t.FirstOffenseDate >= firstDayTwoYearsBefore).
                                                        Where(t => t.FirstOffenseDate <= lastDayOfTheMonth).
                                                        ToList();

            var employeesWithNoTardinessRecordsYet = employeeIds.Except(employeeTardinessRecords.Select(t => t.EmployeeId));

            foreach (var employeeId in employeesWithNoTardinessRecordsYet)

                context.TardinessRecords.Add(new TardinessRecord()
                {
                    EmployeeId = employeeId,
                    FirstOffenseDate = firstDayOfTheMonth,
                    Year = firstDayOfTheMonth.Year
                });

            employeeTardinessRecords = context.TardinessRecords.Local.
                                                Where(t => employeeIds.Contains(t.EmployeeId)).
                                                Where(t => t.FirstOffenseDate >= firstDayTwoYearsBefore).
                                                Where(t => t.FirstOffenseDate <= lastDayOfTheMonth).
                                                ToList();

            if (employeeIds.Count() != employeeTardinessRecords.Count)
                throw new Exception("Error creating new tardiness records.");

            // This will create the employee tardiness date within the year of the reports
            var employeeTardinessDates = CreateEmployeeTardinessDates(employeeTardinessRecords, firstDayOneYear);

            if (employeeIds.Count() != employeeTardinessDates.Count)
                throw new Exception("Error creating new tardiness records.");

            return employeeTardinessDates;
        }

        private List<EmployeeTardinessDate> CreateEmployeeTardinessDates(List<TardinessRecord> employeeTardinessRecords, DateTime firstDayOneYear)
        {
            List<EmployeeTardinessDate> employeeTardinessDates = new List<EmployeeTardinessDate>();
            var groupedTardinessRecords = employeeTardinessRecords.GroupBy(e => e.EmployeeId);

            foreach (var groupedTardinessRecord in groupedTardinessRecords)
            {
                var latestTardinessRecordDate = groupedTardinessRecord.
                                                    OrderByDescending(t => t.FirstOffenseDate).
                                                    FirstOrDefault();

                if (latestTardinessRecordDate == null)
                    continue;

                // if the latest tardiness record is from last 2 years, get its equivalent date for this year
                // Ex: Tardiness Report is Septemper 2019
                // firstDayOneYear would be 10/01/2018
                // if firstOffenseDate is 09/01/2018
                // it will reset to 09/01/2019
                // so the employee will start gaining new tardiness offense on 09/01/2019

                var tardinessRecordDate = latestTardinessRecordDate.FirstOffenseDate;

                if (latestTardinessRecordDate.FirstOffenseDate < firstDayOneYear)
                    tardinessRecordDate = latestTardinessRecordDate.FirstOffenseDate.AddYears(1);

                employeeTardinessDates.Add(new EmployeeTardinessDate()
                {
                    EmployeeId = groupedTardinessRecord.Key,
                    FirstOffenseDate = tardinessRecordDate
                });
            }

            return employeeTardinessDates;
        }

        private static List<CinemaTardinessReportModel> GetSampleTardinessReportModels()
        {
            List<CinemaTardinessReportModel> tardinessReportModels = new List<CinemaTardinessReportModel>();

            tardinessReportModels.Add(new CinemaTardinessReportModel()
            {
                EmployeeName = "Andal, Myrna, B.",
                Days = 2,
                Hours = 4,
                NumberOfOffense = 0
            });

            tardinessReportModels.Add(new CinemaTardinessReportModel()
            {
                EmployeeName = "Banal, Joseph Brian, A.",
                Days = 8,
                Hours = 8.5M,
                NumberOfOffense = 2
            });

            tardinessReportModels.Add(new CinemaTardinessReportModel()
            {
                EmployeeName = "Banga, Jessa, O.",
                Days = 9,
                Hours = 10,
                NumberOfOffense = 4
            });

            tardinessReportModels.Add(new CinemaTardinessReportModel()
            {
                EmployeeName = "Santos, Joshua Noel C.",
                Days = 0,
                Hours = 0,
                NumberOfOffense = 0
            });
            return tardinessReportModels;
        }

        #endregion Private methods

        private class EmployeeTardinessDate
        {
            public int EmployeeId { get; set; }

            private DateTime _firstOffenseDate;

            public DateTime FirstOffenseDate
            {
                get => _firstOffenseDate;
                set =>
                    // always convert to first day of the month
                    _firstOffenseDate = new DateTime(value.Year, value.Month, 1);
            }// First offense within the year
        }
    }
}