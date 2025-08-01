using AccuPay.Core.Enums;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.TimeEntries.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeEntries
{
    public class TimeEntryService
    {
        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ITimeEntryResources _timeEntryResources;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly IOfficialBusinessRepository _officialBusinessRepository;
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ITimeEntryDataService _dataService;

        public TimeEntryService(
            ITimeEntryResources timeEntryResources,
            ICurrentUser currentUser,
            IServiceScopeFactory serviceScopeFactory,
            IEmployeeRepository employeeRepository,
            IPayPeriodRepository payPeriodRepository,
            ILeaveRepository leaveRepository,
            IOvertimeRepository overtimeRepository,
            IOfficialBusinessRepository officialBusinessRepository,
            ITimeLogRepository timeLogRepository,
            ITimeEntryRepository timeEntryRepository,
            IShiftRepository shiftRepository,
            ITimeEntryDataService dataService)
        {
            _timeEntryResources = timeEntryResources;
            _employeeRepository = employeeRepository;
            _payPeriodRepository = payPeriodRepository;
            _currentUser = currentUser;
            _serviceScopeFactory = serviceScopeFactory;
            _leaveRepository = leaveRepository;
            _overtimeRepository = overtimeRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _timeLogRepository = timeLogRepository;
            _timeEntryRepository = timeEntryRepository;
            _shiftRepository = shiftRepository;
            _dataService = dataService;
        }

        public async Task Generate(int payPeriodId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            if (payPeriod == null || payPeriod?.RowID == null || payPeriod?.OrganizationID == null)
                throw new BusinessLogicException("Pay Period does not exists.");

            if (!payPeriod.IsOpen)
                throw new BusinessLogicException("Only \"Open\" pay periods can be computed.");

            if (_timeEntryResources == null)
                throw new BusinessLogicException("Failure loading resources.");

            await _timeEntryResources.Load(_currentUser.OrganizationId, payPeriod.PayFromDate, payPeriod.PayToDate);

            var employees = await _employeeRepository.GetAllActiveAsync(_currentUser.OrganizationId);

            foreach (var employee in employees)
            {
                if (employee?.RowID == null) continue;

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var generator = scope.ServiceProvider.GetRequiredService<ITimeEntryGenerator>();
                    var result = await generator.Start(
                        employee.RowID.Value,
                        _timeEntryResources,
                        _currentUser.OrganizationId,
                        new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate));
                }
            }
        }

        public async Task<PaginatedList<TimeEntryEmployeeDto>> PaginatedEmployeeList(int payPeriodId, PageOptions options, string searchTerm)
        {
            var paginatedList = await _employeeRepository.GetPaginatedListWithTimeEntryAsync(
                options,
                organizationId: _currentUser.OrganizationId,
                payPeriodId: payPeriodId,
                searchTerm);

            return paginatedList.Select(x => TimeEntryEmployeeDto.Convert(x));
        }

        public async Task<ICollection<TimeEntryDto>> GetTimeEntries(int payPeriodId, int employeeId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId); ;

            var list = await _dataService.GetEmployeeTimeEntries(
                _currentUser.OrganizationId,
                employeeId,
                new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate));

            return list.Select(x => TimeEntryDto.Convert(x)).ToList();
        }

        public async Task<TimeEntryPayPeriodDto> GetDetails(int? payPeriodId)
        {
            var periodId = payPeriodId.HasValue ? payPeriodId.Value : 0;
            var payPeriod = await _payPeriodRepository.GetByIdAsync(periodId);

            var datePayPeriod = new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate);

            var timeEntries = await _timeEntryRepository
                .GetByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod);

            var timeEntryCount = timeEntries
                .GroupBy(x => x.EmployeeID)
                .Count();

            var absentCount = timeEntries
                .Where(x => x.AbsentHours >= 0)
                .GroupBy(x => x.EmployeeID)
                .Count();

            var lateCount = timeEntries
                .Where(x => x.LateHours >= 0)
                .GroupBy(x => x.EmployeeID)
                .Count();

            var undertimeCount = timeEntries
                .Where(x => x.UndertimeHours >= 0)
                .GroupBy(x => x.EmployeeID)
                .Count();

            var shiftCount = (await _shiftRepository
                .GetByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod))
                .GroupBy(x => x.EmployeeID)
                .Count();

            var timeLogCount = (await _timeLogRepository
                .GetByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod))
                .GroupBy(x => x.EmployeeID)
                .Count();

            var leaveCount = (await _leaveRepository
                .GetAllApprovedByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod))
                .GroupBy(x => x.EmployeeID)
                .Count();

            var officialBusinessCount = (await _officialBusinessRepository
                .GetAllApprovedByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod))
                .GroupBy(x => x.EmployeeID)
                .Count();

            var overtimeCount = (await _overtimeRepository
                .GetByDatePeriodAsync(_currentUser.OrganizationId, datePayPeriod, OvertimeStatus.Approved))
                .GroupBy(x => x.EmployeeID)
                .Count();

            var dto = new TimeEntryPayPeriodDto();
            dto.ApplyData(payPeriod);

            dto.TimeEntryCount = timeEntryCount;
            dto.ShiftCount = shiftCount;
            dto.TimeLogCount = timeLogCount;
            dto.LeaveCount = leaveCount;
            dto.OfficialBusinessCount = officialBusinessCount;
            dto.OvertimeCount = overtimeCount;
            dto.AbsentCount = absentCount;
            dto.LateCount = lateCount;
            dto.UndertimeCount = undertimeCount;

            return dto;
        }
    }
}
