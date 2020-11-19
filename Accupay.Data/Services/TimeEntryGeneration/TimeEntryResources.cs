using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeEntryResources
    {
        private const int ThreeDays = 3;

        public CalendarCollection CalendarCollection { get; private set; }
        public Organization Organization { get; private set; }
        public PolicyHelper Policy { get; private set; }
        public IReadOnlyCollection<ActualTimeEntry> ActualTimeEntries { get; private set; }
        public IReadOnlyCollection<Agency> Agencies { get; private set; }
        public IReadOnlyCollection<AgencyFee> AgencyFees { get; private set; }
        public IReadOnlyCollection<BreakTimeBracket> BreakTimeBrackets { get; private set; }
        public IReadOnlyCollection<Employee> Employees { get; private set; }
        public IReadOnlyCollection<EmployeeDutySchedule> Shifts { get; private set; }
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

        private readonly ActualTimeEntryRepository _actualTimeEntryRepository;
        private readonly AgencyRepository _agencyRepository;
        private readonly AgencyFeeRepository _agencyFeeRepository;
        private readonly BreakTimeBracketRepository _breakTimeBracketRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly EmploymentPolicyRepository _employmentPolicyRepository;
        private readonly EmployeeDutyScheduleRepository _employeeDutyScheduleRepository;
        private readonly LeaveRepository _leaveRepository;
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private readonly OrganizationRepository _organizationRepository;
        private readonly OvertimeRepository _overtimeRepository;
        private readonly SalaryRepository _salaryRepository;
        private readonly TimeAttendanceLogRepository _timeAttendanceLogRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly TimeLogRepository _timeLogRepository;
        private readonly TripTicketRepository _tripTicketRepository;
        private readonly RouteRateRepository _routeRateRepository;

        public TimeEntryResources(
            CalendarService calendarService,
            PolicyHelper policy,
            ActualTimeEntryRepository actualTimeEntryRepository,
            AgencyRepository agencyRepository,
            AgencyFeeRepository agencyFeeRepository,
            BreakTimeBracketRepository breakTimeBracketRepository,
            EmployeeRepository employeeRepository,
            EmploymentPolicyRepository employmentPolicyRepository,
            EmployeeDutyScheduleRepository employeeDutyScheduleRepository,
            LeaveRepository leaveRepository,
            OfficialBusinessRepository officialBusinessRepository,
            OrganizationRepository organizationRepository,
            OvertimeRepository overtimeRepository,
            SalaryRepository salaryRepository,
            TimeAttendanceLogRepository timeAttendanceLogRepository,
            TimeEntryRepository timeEntryRepository,
            TimeLogRepository timeLogRepository,
            TripTicketRepository tripTicketRepository,
            RouteRateRepository routeRateRepository)
        {
            _calendarService = calendarService;
            Policy = policy;

            _actualTimeEntryRepository = actualTimeEntryRepository;
            _agencyRepository = agencyRepository;
            _agencyFeeRepository = agencyFeeRepository;
            _breakTimeBracketRepository = breakTimeBracketRepository;
            _employeeRepository = employeeRepository;
            _employmentPolicyRepository = employmentPolicyRepository;
            _employeeDutyScheduleRepository = employeeDutyScheduleRepository;
            _leaveRepository = leaveRepository;
            _officialBusinessRepository = officialBusinessRepository;
            _organizationRepository = organizationRepository;
            _overtimeRepository = overtimeRepository;
            _salaryRepository = salaryRepository;
            _timeAttendanceLogRepository = timeAttendanceLogRepository;
            _timeEntryRepository = timeEntryRepository;
            _timeLogRepository = timeLogRepository;
            _tripTicketRepository = tripTicketRepository;
            _routeRateRepository = routeRateRepository;
        }

        public async Task Load(int organizationId, DateTime cutoffStart, DateTime cutoffEnd)
        {
            cutoffStart = cutoffStart.ToMinimumHourValue();
            cutoffEnd = cutoffEnd.ToMinimumHourValue();

            TimePeriod cuttOffPeriod = new TimePeriod(cutoffStart, cutoffEnd);

            Employees = (await _employeeRepository
                .GetAllActiveWithPositionAsync(organizationId))
                .ToList();

            EmploymentPolicies = _employmentPolicyRepository.GetAll().ToList();

            Agencies = _agencyRepository
                .GetAll(organizationId)
                .ToList();

            Organization = _organizationRepository.GetById(organizationId);

            Salaries = _salaryRepository
                .GetByCutOff(organizationId, cutoffEnd)
                .ToList();

            var previousCutoff = PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(cutoffStart);

            DateTime afterCutOff = cutoffEnd;
            if (Policy.PostLegalHolidayCheck)
            {
                afterCutOff = cutoffEnd.AddDays(ThreeDays);
            }

            TimeEntries = _timeEntryRepository
                .GetByDatePeriod(organizationId, new TimePeriod(previousCutoff, afterCutOff))
                .ToList();

            ActualTimeEntries = _actualTimeEntryRepository
                .GetByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            TimeLogs = _timeLogRepository
                .GetByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            Leaves = _leaveRepository
                .GetAllApprovedByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            Overtimes = _overtimeRepository
                .GetByDatePeriod(organizationId, cuttOffPeriod, OvertimeStatus.Approved)
                .ToList();

            OfficialBusinesses = _officialBusinessRepository
                .GetAllApprovedByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            AgencyFees = _agencyFeeRepository
                .GetByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            Shifts = _employeeDutyScheduleRepository
                .GetByDatePeriod(organizationId, cuttOffPeriod)
                .ToList();

            TripTickets = _tripTicketRepository.GetByDateRange(cutoffStart, cutoffEnd).ToList();

            RouteRates = _routeRateRepository.GetAll().ToList();

            if (Policy.ComputeBreakTimeLate)
            {
                TimeAttendanceLogs = _timeAttendanceLogRepository
                    .GetByTimePeriod(organizationId, cuttOffPeriod)
                    .ToList();

                BreakTimeBrackets = _breakTimeBracketRepository
                    .GetAll(organizationId)
                    .ToList();
            }
            else
            {
                TimeAttendanceLogs = new List<TimeAttendanceLog>();
                BreakTimeBrackets = new List<BreakTimeBracket>();
            }

            CalendarCollection = _calendarService
                .GetCalendarCollection(new TimePeriod(previousCutoff, cutoffEnd));
        }
    }
}
