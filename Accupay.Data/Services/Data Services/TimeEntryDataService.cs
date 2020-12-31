using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeEntryDataService
    {
        private readonly BranchRepository _branchRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly EmployeeDutyScheduleRepository _shiftRepository;
        private readonly LeaveRepository _leaveRepository;
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private readonly OvertimeRepository _overtimeRepository;
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly TimeLogRepository _timeLogRepository;
        private readonly TimeEntryDataHelper _timeEntryDataHelper;

        public TimeEntryDataService(
            BranchRepository branchRepository,
            EmployeeRepository employeeRepository,
            EmployeeDutyScheduleRepository shiftRepository,
            LeaveRepository leaveRepository,
            OfficialBusinessRepository officialBusinessRepository,
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            TimeEntryRepository timeEntryRepository,
            TimeLogRepository timeLogRepository,
            TimeEntryDataHelper timeEntryDataHelper)
        {
            _branchRepository = branchRepository;
            _employeeRepository = employeeRepository;
            _leaveRepository = leaveRepository;
            _shiftRepository = shiftRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _overtimeRepository = overtimeRepository;
            _payPeriodRepository = payPeriodRepository;
            _timeEntryRepository = timeEntryRepository;
            _timeLogRepository = timeLogRepository;
            _timeEntryDataHelper = timeEntryDataHelper;
        }

        public async Task DeleteByEmployeeAsync(int employeeId, int payPeriodId, int currentlyLoggedInUserId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);
            if (payPeriod == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new BusinessLogicException("Employee does not exists.");

            if (payPeriod.Status != PayPeriodStatus.Open)
            {
                throw new BusinessLogicException("Cannot delete time entries that is not on an 'Open' pay period.");
            }

            var deletedTimeEntries = await _timeEntryRepository.DeleteByEmployeeAsync(
                employeeId: employeeId,
                payPeriodId: payPeriodId);

            await RecordDeleteByEmployee(currentlyLoggedInUserId, deletedTimeEntries.timeEntries);
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

            var branchIds = timeEntries
                .Where(x => x.BranchID != null)
                .Select(x => x.BranchID.Value)
                .ToArray();
            var branches = await _branchRepository.GetManyByIdsAsync(branchIds);

            // consolidating data
            var timeEntryList = new List<TimeEntryData>();
            timeEntries = timeEntries.OrderBy(x => x.Date).ToList();
            foreach (var timeEntry in timeEntries)
            {
                timeEntryList.Add(new TimeEntryData()
                {
                    Id = timeEntry.RowID.Value,
                    EmployeeId = timeEntry.EmployeeID.Value,
                    Date = timeEntry.Date,
                    TimeEntry = timeEntry,
                    Shift = shifts.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.DateSched == timeEntry.Date),
                    TimeLog = timeLogs.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.LogDate == timeEntry.Date),
                    Overtimes = overtimes.Where(x => x.EmployeeID == timeEntry.EmployeeID && x.OTStartDate == timeEntry.Date).ToList(),
                    OfficialBusiness = officialBusinesses.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.StartDate == timeEntry.Date),
                    Leave = leaves.FirstOrDefault(x => x.EmployeeID == timeEntry.EmployeeID && x.StartDate == timeEntry.Date),
                    Branch = branches.FirstOrDefault(x => x.RowID == timeEntry.BranchID)
                });
            }

            return timeEntryList;
        }

        public async Task RecordCreateByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await _timeEntryDataHelper.RecordCreate(currentlyLoggedInUserId, timeEntries);
        }

        public async Task RecordEditByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await _timeEntryDataHelper.RecordEdit(currentlyLoggedInUserId, timeEntries);
        }

        public async Task RecordDeleteByEmployee(int currentlyLoggedInUserId, IReadOnlyCollection<TimeEntry> timeEntries)
        {
            await _timeEntryDataHelper.RecordDelete(currentlyLoggedInUserId, timeEntries);
        }
    }
}
