using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeEntryDataService
    {
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly EmployeeDutyScheduleRepository _shiftRepository;
        private readonly TimeLogRepository _timeLogRepository;
        private readonly OvertimeRepository _overtimeRepository;
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private readonly LeaveRepository _leaveRepository;
        private readonly BranchRepository _branchRepository;

        public TimeEntryDataService(
            TimeEntryRepository timeEntryRepository,
            EmployeeDutyScheduleRepository shiftRepository,
            TimeLogRepository timeLogRepository,
            OvertimeRepository overtimeRepository,
            OfficialBusinessRepository officialBusinessRepository,
            LeaveRepository leaveRepository,
            BranchRepository branchRepository)
        {
            _timeEntryRepository = timeEntryRepository;
            _shiftRepository = shiftRepository;
            _timeLogRepository = timeLogRepository;
            _overtimeRepository = overtimeRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _leaveRepository = leaveRepository;
            _branchRepository = branchRepository;
        }

        public async Task<ICollection<TimeEntryData>> GetEmployeeTimeEntries(int organizationId, int employeeId, TimePeriod datePeriod)
        {
            var timeEntries = await _timeEntryRepository.GetByEmployeeAndDatePeriodAsync(
                organizationId: organizationId,
                employeeId: employeeId,
                datePeriod: datePeriod);

            var timeEntryIds = timeEntries.Select(x => x.RowID.Value).ToArray();

            var shifts = await _shiftRepository.GetByEmployeeAndDatePeriodAsync(
                organizationId: organizationId,
                employeeId: employeeId,
                datePeriod: datePeriod);

            var timeLogs = await _timeLogRepository.GetLatestByEmployeeAndDatePeriodAsync(employeeId, datePeriod);

            var overtimes = await _overtimeRepository.GetByEmployeeAndDatePeriod(
                organizationId: organizationId,
                employeeId: employeeId,
                datePeriod: datePeriod);

            var officialBusinesses = await _officialBusinessRepository.GetAllApprovedByEmployeeAndDatePeriodAsync(
                organizationId: organizationId,
                employeeId: employeeId,
                datePeriod: datePeriod);

            var leaves = await _leaveRepository.GetAllApprovedByEmployeeAndDatePeriod(
                organizationId: organizationId,
                employeeId: employeeId,
                datePeriod: datePeriod);

            var branches = await _branchRepository.GetByMultipleIds(timeEntryIds);

            // consolidating data
            var timeEntryList = new List<TimeEntryData>();
            timeEntries = timeEntries.OrderBy(x => x.Date).ToList();
            foreach (var timeEntry in timeEntries)
            {
                timeEntryList.Add(new TimeEntryData()
                {
                    EmployeeId = timeEntry.EmployeeID.Value,
                    Date = timeEntry.Date,
                    TimeEntry = timeEntry,
                    Shift = shifts.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.DateSched == timeEntry.Date),
                    TimeLog = timeLogs.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.LogDate == timeEntry.Date),
                    Overtime = overtimes.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.OTStartDate == timeEntry.Date),
                    OfficialBusiness = officialBusinesses.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.StartDate == timeEntry.Date),
                    Leave = leaves.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.StartDate == timeEntry.Date),
                });
            }

            return timeEntryList;
        }
    }
}