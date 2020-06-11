using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.ValueObjects;
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
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly TimeEntryGenerator _generator;
        private readonly ICurrentUser _currentUser;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly LeaveRepository _leaveRepository;
        private readonly OvertimeRepository _overtimeRepository;
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private readonly TimeLogRepository _timeLogRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly EmployeeDutyScheduleRepository _shiftRepository;
        private readonly TimeEntryDataService _dataService;

        public TimeEntryService(EmployeeRepository employeeRepository,
                                PayPeriodRepository payPeriodRepository,
                                TimeEntryGenerator generator,
                                ICurrentUser currentUser,
                                IServiceScopeFactory serviceScopeFactory,
                                LeaveRepository leaveRepository,
                                OvertimeRepository overtimeRepository,
                                OfficialBusinessRepository officialBusinessRepository,
                                TimeLogRepository timeLogRepository,
                                TimeEntryRepository timeEntryRepository,
                                EmployeeDutyScheduleRepository shiftRepository,
                                TimeEntryDataService dataService)
        {
            _employeeRepository = employeeRepository;
            _payPeriodRepository = payPeriodRepository;
            _generator = generator;
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
            //using (var scope = _serviceScopeFactory.CreateScope())
            //{
            //    var generator = scope.ServiceProvider.GetRequiredService<TimeEntryGenerator>();

            //    await Task.Run(() => generator.Start(_currentUser.OrganizationId, payPeriod.PayFromDate, payPeriod.PayToDate));
            //}

            _generator.Start(_currentUser.OrganizationId, payPeriod.PayFromDate, payPeriod.PayToDate);
        }

        public async Task<PaginatedList<TimeEntryEmployeeDto>> PaginatedEmployeeList(int payPeriodId, PageOptions options, string searchTerm)
        {
            var paginatedList = await _employeeRepository.GetPaginatedListWithTimeEntryAsync(
                options,
                organizationId: _currentUser.OrganizationId,
                payPeriodId: payPeriodId,
                searchTerm);

            var dtos = paginatedList.List.Select(x => TimeEntryEmployeeDto.Convert(x));

            return new PaginatedList<TimeEntryEmployeeDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<ICollection<TimeEntryDto>> GetTimeEntries(int payPeriodId, int employeeId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var list = await _dataService.GetEmployeeTimeEntries(
                _currentUser.OrganizationId,
                employeeId,
                new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate));

            return list.Select(x => TimeEntryDto.Convert(x)).ToList();
        }

        public async Task<TimeEntryPayPeriodDto> GetDetails(int payPeriodId)
        {
            var payPeriod = await _payPeriodRepository.GetByIdAsync(payPeriodId);

            var datePayPeriod = new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate);

            var timeEntries = _timeEntryRepository
                                .GetByDatePeriod(_currentUser.OrganizationId, datePayPeriod);

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

            var shiftCount = _shiftRepository
                                .GetByDatePeriod(_currentUser.OrganizationId, datePayPeriod)
                                .GroupBy(x => x.EmployeeID)
                                .Count();

            var timeLogCount = _timeLogRepository
                                .GetByDatePeriod(_currentUser.OrganizationId, datePayPeriod)
                                .GroupBy(x => x.EmployeeID)
                                .Count();

            var leaveCount = _leaveRepository
                                .GetAllApprovedByDatePeriod(_currentUser.OrganizationId, datePayPeriod)
                                .GroupBy(x => x.EmployeeID)
                                .Count();

            var officialBusinessCount = _officialBusinessRepository
                                .GetAllApprovedByDatePeriod(_currentUser.OrganizationId, datePayPeriod)
                                .GroupBy(x => x.EmployeeID)
                                .Count();

            var overtimeCount = _overtimeRepository
                                .GetByDatePeriod(_currentUser.OrganizationId, datePayPeriod, Data.Enums.OvertimeStatus.Approved)
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
