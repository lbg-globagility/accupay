using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TimeEntryDataService : ITimeEntryDataService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IOfficialBusinessRepository _officialBusinessRepository;
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly TimeEntryDataHelper _timeEntryDataHelper;

        public TimeEntryDataService(
            IBranchRepository branchRepository,
            IEmployeeRepository employeeRepository,
            ILeaveRepository leaveRepository,
            IOfficialBusinessRepository officialBusinessRepository,
            IOvertimeRepository overtimeRepository,
            IPayPeriodRepository payPeriodRepository,
            IShiftRepository shiftRepository,
            ITimeEntryRepository timeEntryRepository,
            ITimeLogRepository timeLogRepository,
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
