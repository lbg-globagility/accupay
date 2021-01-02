using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Repositories;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TimeEntryResources
    {
        private const int ThreeDays = 3;

        public CalendarCollection CalendarCollection { get; private set; }
        public Organization Organization { get; private set; }
        public IPolicyHelper Policy { get; private set; }
        public IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries { get; private set; }
        public IReadOnlyCollection<Agency> Agencies { get; private set; }
        public IReadOnlyCollection<AgencyFee> AgencyFees { get; private set; }
        public IReadOnlyCollection<BreakTimeBracket> BreakTimeBrackets { get; private set; }
        public IReadOnlyCollection<Employee> Employees { get; private set; }
        public IReadOnlyCollection<Shift> Shifts { get; private set; }
        public IReadOnlyCollection<EmploymentPolicy> EmploymentPolicies { get; private set; }
        public IReadOnlyCollection<Leave> Leaves { get; private set; }
        public IReadOnlyCollection<OfficialBusiness> OfficialBusinesses { get; private set; }
        public IReadOnlyCollection<Overtime> Overtimes { get; private set; }
        public IReadOnlyCollection<RoutePayRate> RouteRates { get; private set; }
        public IReadOnlyCollection<Salary> Salaries { get; private set; }
        public IReadOnlyCollection<TimeAttendanceLog> TimeAttendanceLogs { get; private set; }
        public IReadOnlyCollection<TimeEntry> TimeEntries { get; private set; }
        public IReadOnlyCollection<TimeLog> TimeLogs { get; private set; }
        public IReadOnlyCollection<TripTicket> TripTickets { get; private set; }

        private readonly CalendarService _calendarService;

        private readonly IActualTimeEntryRepository _actualTimeEntryRepository;
        private readonly IAgencyRepository _agencyRepository;
        private readonly IAgencyFeeRepository _agencyFeeRepository;
        private readonly IBreakTimeBracketRepository _breakTimeBracketRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmploymentPolicyRepository _employmentPolicyRepository;
        private readonly ILeaveRepository _leaveRepository;
        private readonly IOfficialBusinessRepository _officialBusinessRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly ISalaryRepository _salaryRepository;
        private readonly IRouteRateRepository _routeRateRepository;
        private readonly IShiftRepository _shiftRepository;
        private readonly ITimeAttendanceLogRepository _timeAttendanceLogRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly TimeLogRepository _timeLogRepository;
        private readonly TripTicketRepository _tripTicketRepository;

        public TimeEntryResources(
            CalendarService calendarService,
            IPolicyHelper policy,
            IActualTimeEntryRepository actualTimeEntryRepository,
            IAgencyRepository agencyRepository,
            IAgencyFeeRepository agencyFeeRepository,
            IBreakTimeBracketRepository breakTimeBracketRepository,
            IEmployeeRepository employeeRepository,
            IEmploymentPolicyRepository employmentPolicyRepository,
            ILeaveRepository leaveRepository,
            IOfficialBusinessRepository officialBusinessRepository,
            IOrganizationRepository organizationRepository,
            IOvertimeRepository overtimeRepository,
            IRouteRateRepository routeRateRepository,
            ISalaryRepository salaryRepository,
            IShiftRepository shiftRepository,
            ITimeAttendanceLogRepository timeAttendanceLogRepository,
            TimeEntryRepository timeEntryRepository,
            TimeLogRepository timeLogRepository,
            TripTicketRepository tripTicketRepository)
        {
            _calendarService = calendarService;
            Policy = policy;

            _actualTimeEntryRepository = actualTimeEntryRepository;
            _agencyRepository = agencyRepository;
            _agencyFeeRepository = agencyFeeRepository;
            _breakTimeBracketRepository = breakTimeBracketRepository;
            _employeeRepository = employeeRepository;
            _employmentPolicyRepository = employmentPolicyRepository;
            _shiftRepository = shiftRepository;
            _leaveRepository = leaveRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _organizationRepository = organizationRepository;
            _overtimeRepository = overtimeRepository;
            _routeRateRepository = routeRateRepository;
            _salaryRepository = salaryRepository;
            _timeAttendanceLogRepository = timeAttendanceLogRepository;
            _timeEntryRepository = timeEntryRepository;
            _timeLogRepository = timeLogRepository;
            _tripTicketRepository = tripTicketRepository;
        }

        public async Task Load(int organizationId, DateTime cutoffStart, DateTime cutoffEnd)
        {
            cutoffStart = cutoffStart.ToMinimumHourValue();
            cutoffEnd = cutoffEnd.ToMinimumHourValue();

            TimePeriod cuttOffPeriod = new TimePeriod(cutoffStart, cutoffEnd);
            var previousCutoff = PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(cutoffStart);

            await LoadActualTimeEntries(organizationId, cuttOffPeriod);
            await LoadAgencies(organizationId);
            await LoadAgencyFees(organizationId, cuttOffPeriod);
            await LoadBreakTimeBrackets(organizationId);
            await LoadCalendarCollection(cutoffEnd, previousCutoff);
            await LoadEmployeePolicies();
            await LoadEmployees(organizationId);
            await LoadLeaves(organizationId, cuttOffPeriod);
            await LoadOfficialBusinesses(organizationId, cuttOffPeriod);
            await LoadOrganization(organizationId);
            await LoadOvertimes(organizationId, cuttOffPeriod);
            await LoadRouteRates();
            await LoadSalaries(organizationId, cutoffEnd);
            await LoadShifts(organizationId, cuttOffPeriod);
            await LoadTimeAttendanceLogs(organizationId, cuttOffPeriod);
            await LoadTimeEntries(organizationId, previousCutoff, cutoffEnd);
            await LoadTimeLogs(organizationId, cuttOffPeriod);
            await LoadTripTickets(cuttOffPeriod);
        }

        private async Task LoadActualTimeEntries(int organizationId, TimePeriod cuttOffPeriod)
        {
            ActualTimeEntries = (await _actualTimeEntryRepository
                .GetByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadAgencies(int organizationId)
        {
            Agencies = (await _agencyRepository
                .GetAllAsync(organizationId))
                .ToList();
        }

        private async Task LoadAgencyFees(int organizationId, TimePeriod cuttOffPeriod)
        {
            AgencyFees = (await _agencyFeeRepository
                .GetByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadBreakTimeBrackets(int organizationId)
        {
            if (Policy.ComputeBreakTimeLate)
            {
                BreakTimeBrackets = (await _breakTimeBracketRepository
                    .GetAllAsync(organizationId))
                    .ToList();
            }
            else
            {
                BreakTimeBrackets = new List<BreakTimeBracket>();
            }
        }

        private async Task LoadCalendarCollection(DateTime cutoffEnd, DateTime previousCutoff)
        {
            CalendarCollection = await _calendarService
                .GetCalendarCollectionAsync(new TimePeriod(previousCutoff, cutoffEnd));
        }

        private async Task LoadEmployeePolicies()
        {
            EmploymentPolicies = (await _employmentPolicyRepository.GetAllAsync()).ToList();
        }

        private async Task LoadEmployees(int organizationId)
        {
            Employees = (await _employeeRepository
                .GetAllActiveWithPositionAsync(organizationId))
                .ToList();
        }

        private async Task LoadLeaves(int organizationId, TimePeriod cuttOffPeriod)
        {
            Leaves = (await _leaveRepository
                .GetAllApprovedByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadOfficialBusinesses(int organizationId, TimePeriod cuttOffPeriod)
        {
            OfficialBusinesses = (await _officialBusinessRepository
                .GetAllApprovedByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadOrganization(int organizationId)
        {
            Organization = await _organizationRepository.GetByIdAsync(organizationId);
        }

        private async Task LoadOvertimes(int organizationId, TimePeriod cuttOffPeriod)
        {
            Overtimes = (await _overtimeRepository
                .GetByDatePeriodAsync(organizationId, cuttOffPeriod, OvertimeStatus.Approved))
                .ToList();
        }

        private async Task LoadRouteRates()
        {
            RouteRates = (await _routeRateRepository
                .GetAllAsync())
                .ToList();
        }

        private async Task LoadSalaries(int organizationId, DateTime cutoffEnd)
        {
            Salaries = (await _salaryRepository
                .GetByCutOffAsync(organizationId, cutoffEnd))
                .ToList();
        }

        private async Task LoadShifts(int organizationId, TimePeriod cuttOffPeriod)
        {
            Shifts = (await _shiftRepository
                .GetByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadTimeAttendanceLogs(int organizationId, TimePeriod cuttOffPeriod)
        {
            if (Policy.ComputeBreakTimeLate)
            {
                TimeAttendanceLogs = (await _timeAttendanceLogRepository
                    .GetByTimePeriodAsync(organizationId, cuttOffPeriod))
                    .ToList();
            }
            else
            {
                TimeAttendanceLogs = new List<TimeAttendanceLog>();
            }
        }

        private async Task LoadTimeEntries(int organizationId, DateTime previousCutoff, DateTime cutoffEnd)
        {
            DateTime afterCutOff = cutoffEnd;
            if (Policy.PostLegalHolidayCheck)
            {
                afterCutOff = cutoffEnd.AddDays(ThreeDays);
            }

            TimeEntries = (await _timeEntryRepository
                .GetByDatePeriodAsync(organizationId, new TimePeriod(previousCutoff, afterCutOff)))
                .ToList();
        }

        private async Task LoadTimeLogs(int organizationId, TimePeriod cuttOffPeriod)
        {
            TimeLogs = (await _timeLogRepository
                .GetByDatePeriodAsync(organizationId, cuttOffPeriod))
                .ToList();
        }

        private async Task LoadTripTickets(TimePeriod cuttOffPeriod)
        {
            TripTickets = (await _tripTicketRepository
                .GetByDateRangeAsync(cuttOffPeriod))
                .ToList();
        }
    }
}
