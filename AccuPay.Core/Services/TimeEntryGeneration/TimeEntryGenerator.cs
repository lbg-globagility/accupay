using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TimeEntryGenerator
    {
        //private static ILog logger = LogManager.GetLogger("TimeEntryLogger", "TimeEntryLogger");

        private readonly PayrollContext _context;
        private readonly TimeEntryDataService _timeEntryDataService;

        public TimeEntryGenerator(PayrollContext context, TimeEntryDataService timeEntryDataService)
        {
            _context = context;
            _timeEntryDataService = timeEntryDataService;
        }

        public async Task<EmployeeResult> Start(
            int employeeId,
            TimeEntryResources resources,
            int currentlyLoggedInUserId,
            TimePeriod payPeriod)
        {
            // we use the employee data from resources.Employees instead of just passing the employee
            // entity in the Start method because we can be sure that the data in resources.Employees
            // are complete employee data (ex. with Position, Divisition) that are needed by PayrollGenerator.
            var employee = resources.Employees.Where(x => x.RowID == employeeId).FirstOrDefault();

            if (employee == null)
            {
                throw new Exception("Employee was not loaded.");
            }

            var previousTimeEntries = resources.TimeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            ICollection<ActualTimeEntry> actualTimeEntries = resources.ActualTimeEntries
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var salary = resources.Salaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);

            IEmploymentPolicy employmentPolicy = resources.EmploymentPolicies.FirstOrDefault(t => t.Id == employee.EmploymentPolicyId);
            if (employmentPolicy is null)
            {
                employmentPolicy = new SubstituteEmploymentPolicy(employee);
            }

            var timeLogs = resources.TimeLogs
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            var overtimesInCutoff = resources.Overtimes
                .Where(o => o.EmployeeID == employee.RowID)
                .ToList();

            var officialBusinesses = resources.OfficialBusinesses
                .Where(o => o.EmployeeID == employee.RowID)
                .ToList();

            var leavesInCutoff = resources.Leaves.Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.LeaveType != "Leave w/o Pay")
                .ToList();

            ICollection<AgencyFee> agencyFees = resources.AgencyFees
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var shifts = resources.Shifts
                .Where(es => es.EmployeeID == employee.RowID)
                .ToList();

            var timeAttendanceLogs = resources.TimeAttendanceLogs
                 .Where(t => t.EmployeeID == employee.RowID)
                 .ToList();

            var breakTimeBrackets = resources.BreakTimeBrackets
                .Where(b => b.DivisionID == employee.Position?.DivisionID)
                .ToList();

            var tripTickets = resources.TripTickets.Where(
                t => t.Employees
                    .Any(u => u.EmployeeID == employee.RowID))
                .ToList();

            if (employee.IsActive == false)
            {
                var currentTimeEntries = previousTimeEntries
                    .Where(t => payPeriod.Start <= t.Date && t.Date <= payPeriod.End);

                if (!currentTimeEntries.Any())
                {
                    return EmployeeResult.Error(employee, "Employee is no longer active.");
                }
            }

            // If there aren't any attendance data, that means there aren't any time entries to compute.
            if (!(timeLogs.Any() || leavesInCutoff.Any() || officialBusinesses.Any()) && (!employee.IsFixed))
            {
                return EmployeeResult.Error(employee, "Employee does not have any time logs, leaves or official business filed for this cutoff.");
            }

            if (employee.StartDate.Date > payPeriod.End.Date)
            {
                return EmployeeResult.Error(employee, "Employee start date is greater than the cut off end date.");
            }

            if (salary == null)
            {
                return EmployeeResult.Error(employee, "Employee has no salary for this cut off.");
            }

            if (employee.Position == null)
            {
                return EmployeeResult.Error(employee, "Employee has no job position set.");
            }

            var dayCalculator = new DayCalculator(employee, employmentPolicy, resources.Organization, resources.Policy);

            var timeEntries = new List<TimeEntry>();
            var regularHolidaysList = new List<DateTime>(); // Used for postlegalholidaycheck

            foreach (var currentDate in CalendarHelper.EachDay(payPeriod.Start, payPeriod.End))
            {
                try
                {
                    // TODO: employeetimeentrydetails date should be unique so no query like this should be needed.
                    var timelog = timeLogs.OrderByDescending(t => t.LastUpd).FirstOrDefault(t => t.LogDate == currentDate);

                    var overtimes = overtimesInCutoff.Where(o => o.OTStartDate <= currentDate && currentDate <= o.OTEndDate).ToList();
                    var leaves = leavesInCutoff.Where(l => l.StartDate == currentDate).ToList();
                    var officialBusiness = officialBusinesses.FirstOrDefault(o => o.StartDate.Value == currentDate);
                    var shift = shifts.FirstOrDefault(es => es.DateSched == currentDate);
                    var currentTimeAttendanceLogs = timeAttendanceLogs.Where(l => l.WorkDay == currentDate).ToList();
                    var tripTicketsForDate = tripTickets.Where(t => t.Date == currentDate).ToList();

                    var branchId = timelog?.BranchID ?? employee?.BranchID;
                    var payrate = resources.CalendarCollection.GetCalendar(branchId).Find(currentDate);

                    var timeEntry = dayCalculator.Compute(
                        currentDate,
                        salary,
                        previousTimeEntries,
                        shift,
                        timelog,
                        overtimes,
                        officialBusiness,
                        leaves,
                        currentTimeAttendanceLogs,
                        breakTimeBrackets,
                        payrate,
                        resources.CalendarCollection,
                        branchId,
                        tripTicketsForDate,
                        resources.RouteRates);

                    if (payrate.IsRegularHoliday)
                    {
                        regularHolidaysList.Add(currentDate);
                    }

                    // this is for the issue on hasWorkedLastDay on first time entry generation.
                    // since there is no oldTimeEntries yet, hasWorkedLastDay will always be false.
                    if (previousTimeEntries.Where(x => x.EmployeeID == employee.RowID)
                        .Where(x => x.Date == currentDate)
                        .Any() == false)
                    {
                        previousTimeEntries.Add(timeEntry);
                    }

                    timeEntries.Add(timeEntry);
                }
                catch (Exception ex)
                {
                    return EmployeeResult.Error(employee, $"Failure to generate time entries for employee {employee.EmployeeNo} {ex.Message}");
                }
            }

            PostLegalHolidayCheck(employee, timeEntries, resources.Policy, regularHolidaysList, resources.CalendarCollection, payPeriod.End);
            timeEntries.ForEach(t =>
            {
                t.RegularHolidayPay += t.BasicRegularHolidayPay;
                t.TotalDayPay += t.BasicRegularHolidayPay;
            });

            if (employee.IsUnderAgency)
            {
                var agency = resources.Agencies.SingleOrDefault(a => a.RowID == employee.AgencyID);

                var agencyCalculator = new AgencyFeeCalculator(employee, agency, agencyFees);
                agencyFees = agencyCalculator.Compute(timeEntries);
            }

            var actualTimeEntryCalculator = new ActualTimeEntryCalculator(salary, actualTimeEntries, resources.Policy);
            actualTimeEntries = actualTimeEntryCalculator.Compute(timeEntries);

            // TODO: move this to a repository and data service, then move the useractivity to that data service
            var newTimeEntries = timeEntries.Where(x => x.IsNewEntity).ToList();
            var updatedTimeEntries = timeEntries.Where(x => !x.IsNewEntity).ToList();

            AddTimeEntriesToContext(currentlyLoggedInUserId, timeEntries);
            AddActualTimeEntriesToContext(actualTimeEntries);
            AddAgencyFeesToContext(currentlyLoggedInUserId, agencyFees);
            await _context.SaveChangesAsync();

            await _timeEntryDataService.RecordCreateByEmployee(currentlyLoggedInUserId, newTimeEntries);
            await _timeEntryDataService.RecordEditByEmployee(currentlyLoggedInUserId, updatedTimeEntries);

            return EmployeeResult.Success(employee);
        }

        private void PostLegalHolidayCheck(
            Employee employee,
            List<TimeEntry> timeEntries,
            IPolicyHelper policy,
            List<DateTime> regularHolidaysList,
            CalendarCollection calendarCollection,
            DateTime cutOffEnd)
        {
            if (policy.PostLegalHolidayCheck)
            {
                if (!employee.CalcHoliday)
                    return;

                if (regularHolidaysList.Any())
                {
                    foreach (var holidayDate in regularHolidaysList)
                    {
                        var presentAfterLegalHoliday = PayrollTools.HasWorkAfterLegalHoliday(
                            holidayDate,
                            cutOffEnd,
                            timeEntries,
                            calendarCollection);

                        if (!presentAfterLegalHoliday)
                        {
                            var timeEntry = timeEntries
                                .Where(t => t.Date == holidayDate)
                                .FirstOrDefault();

                            timeEntry.BasicRegularHolidayPay = 0;
                        }
                    }
                }
            }
        }

        private void AddTimeEntriesToContext(int currentlyLoggedInUserId, IList<TimeEntry> timeEntries)
        {
            foreach (var timeEntry in timeEntries)
            {
                if (timeEntry.RowID.HasValue)
                {
                    timeEntry.LastUpdBy = currentlyLoggedInUserId;
                    _context.Entry(timeEntry).State = EntityState.Modified;
                }
                else
                {
                    timeEntry.CreatedBy = currentlyLoggedInUserId;
                    _context.TimeEntries.Add(timeEntry);
                }
            }
        }

        private void AddActualTimeEntriesToContext(ICollection<ActualTimeEntry> actualTimeEntries)
        {
            foreach (var actualTimeEntry in actualTimeEntries)
            {
                if (actualTimeEntry.RowID.HasValue)
                    _context.Entry(actualTimeEntry).State = EntityState.Modified;
                else
                    _context.ActualTimeEntries.Add(actualTimeEntry);
            }
        }

        private void AddAgencyFeesToContext(int currentlyLoggedInUserId, ICollection<AgencyFee> agencyFees)
        {
            foreach (var agencyFee in agencyFees)
            {
                if (agencyFee.RowID.HasValue)
                {
                    agencyFee.LastUpdBy = currentlyLoggedInUserId;
                    _context.Entry(agencyFee).State = EntityState.Modified;
                }
                else
                {
                    agencyFee.CreatedBy = currentlyLoggedInUserId;
                    _context.AgencyFees.Add(agencyFee);
                }
            }
        }
    }
}
