using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeEntryGenerator
    {
        //private static ILog logger = LogManager.GetLogger("TimeEntryLogger", "TimeEntryLogger");

        private int _organizationId;
        private DateTime _cutoffStart;
        private DateTime _cutoffEnd;
        private int _threeDays = 3;
        private IList<TimeEntry> _timeEntries;
        private IList<ActualTimeEntry> _actualTimeEntries;
        private IList<TimeLog> _timeLogs;
        private IList<Overtime> _overtimes;
        private IList<Leave> _leaves;
        private IList<OfficialBusiness> _officialBusinesses;
        private IList<AgencyFee> _agencyFees;
        private IList<ShiftSchedule> _employeeShifts;
        private IList<Salary> _salaries;
        private ICollection<EmploymentPolicy> _employmentPolicies;
        private IList<EmployeeDutySchedule> _shiftSchedules;
        private List<TimeAttendanceLog> _timeAttendanceLogs;
        private List<BreakTimeBracket> _breakTimeBrackets;
        private readonly DbContextOptionsService _dbContextOptionsService;
        private readonly CalendarService _calendarService;
        private readonly ListOfValueService _listOfValueService;

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
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly SalaryRepository _salaryRepository;
        private readonly ShiftScheduleRepository _shiftScheduleRepository;
        private readonly TimeAttendanceLogRepository _timeAttendanceLogRepository;
        private readonly TimeEntryRepository _timeEntryRepository;
        private readonly TimeLogRepository _timeLogRepository;

        private int _total;

        private int _finished;

        private int _errors;

        public int ErrorCount => _errors;

        public int Progress
        {
            get
            {
                if (_finished == 0)
                    return 0;

                //using decimal does not update the progress threading
                return Convert.ToInt32(Math.Floor(_finished / (double)_total * 100));
            }
        }

        public TimeEntryGenerator(
            DbContextOptionsService dbContextOptionsService,
            CalendarService calendarService,
            ListOfValueService listOfValueService,
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
            PayPeriodRepository payPeriodRepository,
            SalaryRepository salaryRepository,
            ShiftScheduleRepository shiftScheduleRepository,
            TimeAttendanceLogRepository timeAttendanceLogRepository,
            TimeEntryRepository timeEntryRepository,
            TimeLogRepository timeLogRepository)
        {
            _dbContextOptionsService = dbContextOptionsService;
            _calendarService = calendarService;
            _listOfValueService = listOfValueService;
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
            _payPeriodRepository = payPeriodRepository;
            _salaryRepository = salaryRepository;
            _shiftScheduleRepository = shiftScheduleRepository;
            _timeAttendanceLogRepository = timeAttendanceLogRepository;
            _timeEntryRepository = timeEntryRepository;
            _timeLogRepository = timeLogRepository;
        }

        public void Start(int organizationId, int userId, DateTime cutoffStart, DateTime cutoffEnd)
        {
            _organizationId = organizationId;
            _cutoffStart = cutoffStart.ToMinimumHourValue();
            _cutoffEnd = cutoffEnd.ToMinimumHourValue();

            var currentPayPeriod = _payPeriodRepository.GetCurrentOpen(organizationId);

            if (currentPayPeriod?.PayFromDate != cutoffStart || currentPayPeriod?.PayToDate != cutoffEnd)
                throw new BusinessLogicException("Only open pay periods can generate time entries.");

            ListOfValueCollection settings = _listOfValueService.Create();
            TimeEntryPolicy timeEntryPolicy = new TimeEntryPolicy(settings);

            TimePeriod cuttOffPeriod = new TimePeriod(_cutoffStart, _cutoffEnd);

            ICollection<Employee> employees = _employeeRepository
                .GetAllActiveWithPosition(_organizationId)
                .ToList();

            _employmentPolicies = _employmentPolicyRepository.GetAll();

            ICollection<Agency> agencies = _agencyRepository
                .GetAll(_organizationId)
                .ToList();

            var organization = _organizationRepository.GetById(_organizationId);

            _salaries = _salaryRepository
                .GetByCutOff(_organizationId, _cutoffStart)
                .ToList();

            var previousCutoff = PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(_cutoffStart);

            DateTime afterCutOff = _cutoffEnd;
            if (timeEntryPolicy.PostLegalHolidayCheck)
            {
                afterCutOff = _cutoffEnd.AddDays(_threeDays);
            }

            _timeEntries = _timeEntryRepository
                .GetByDatePeriod(_organizationId, new TimePeriod(previousCutoff, afterCutOff))
                .ToList();

            _actualTimeEntries = _actualTimeEntryRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _timeLogs = _timeLogRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _leaves = _leaveRepository
                .GetAllApprovedByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _overtimes = _overtimeRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod, OvertimeStatus.Approved)
                .ToList();

            _officialBusinesses = _officialBusinessRepository
                .GetAllApprovedByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _agencyFees = _agencyFeeRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _employeeShifts = _shiftScheduleRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            _shiftSchedules = _employeeDutyScheduleRepository
                .GetByDatePeriod(_organizationId, cuttOffPeriod)
                .ToList();

            if (timeEntryPolicy.ComputeBreakTimeLate)
            {
                _timeAttendanceLogs = _timeAttendanceLogRepository
                    .GetByTimePeriod(_organizationId, cuttOffPeriod)
                    .ToList();

                _breakTimeBrackets = _breakTimeBracketRepository
                    .GetAll(_organizationId)
                    .ToList();
            }
            else
            {
                _timeAttendanceLogs = new List<TimeAttendanceLog>();
                _breakTimeBrackets = new List<BreakTimeBracket>();
            }

            var payrateCalculationBasis = settings.GetEnum("Pay rate.CalculationBasis", PayRateCalculationBasis.Organization);

            CalendarCollection calendarCollection = _calendarService
                .GetCalendarCollection(
                    new TimePeriod(previousCutoff, _cutoffEnd),
                    payrateCalculationBasis,
                    _organizationId);

            var progress = new ObservableCollection<int>();

            _total = employees.Count;

            // TEMPORARY set to synchronous since there is a race condition issue
            // that is hard to debug
            employees.ToList().ForEach(employee =>
            {
                try
                {
                    CalculateEmployeeEntries(employee, organization, userId, settings, agencies, timeEntryPolicy, calendarCollection);
                }
                catch (Exception ex)
                {
                    // can error if employee type is null
                    //logger.Error(ex.Message, ex);
                    _errors += 1;
                }

                Interlocked.Increment(ref _finished);
            });
        }

        private void CalculateEmployeeEntries(
            Employee employee,
            Organization organization,
            int userId,
            ListOfValueCollection settings,
            ICollection<Agency> agencies,
            TimeEntryPolicy timeEntryPolicy,
            CalendarCollection calendarCollection)
        {
            var previousTimeEntries = _timeEntries
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            ICollection<ActualTimeEntry> actualTimeEntries = _actualTimeEntries
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var salary = _salaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);

            IEmploymentPolicy employmentPolicy = _employmentPolicies.FirstOrDefault(t => t.Id == employee.EmploymentPolicyId);
            if (employmentPolicy is null)
            {
                employmentPolicy = new SubstituteEmploymentPolicy(employee);
            }

            var timeLogs = _timeLogs
                .Where(t => t.EmployeeID == employee.RowID)
                .ToList();

            var shiftSchedules = _employeeShifts
                .Where(s => s.EmployeeID == employee.RowID)
                .ToList();

            var overtimesInCutoff = _overtimes
                .Where(o => o.EmployeeID == employee.RowID)
                .ToList();

            var officialBusinesses = _officialBusinesses
                .Where(o => o.EmployeeID == employee.RowID)
                .ToList();

            var leavesInCutoff = _leaves.Where(l => l.EmployeeID == employee.RowID)
                .Where(l => l.LeaveType != "Leave w/o Pay")
                .ToList();

            ICollection<AgencyFee> agencyFees = _agencyFees
                .Where(a => a.EmployeeID == employee.RowID)
                .ToList();

            var dutyShiftSchedules = _shiftSchedules
                .Where(es => es.EmployeeID == employee.RowID)
                .ToList();

            var timeAttendanceLogs = _timeAttendanceLogs
                 .Where(t => t.EmployeeID == employee.RowID)
                 .ToList();

            var breakTimeBrackets = _breakTimeBrackets
                .Where(b => b.DivisionID == employee.Position?.DivisionID)
                .ToList();

            if (employee.IsActive == false)
            {
                var currentTimeEntries = previousTimeEntries
                    .Where(t => _cutoffStart <= t.Date && t.Date <= _cutoffEnd);

                // TODO: return this as one the list of errors of Time entry generation
                if (!currentTimeEntries.Any())
                    return;
            }

            // TODO: return this as one the list of errors of Time entry generation
            if (!(timeLogs.Any() || leavesInCutoff.Any() || officialBusinesses.Any()) && (!employee.IsFixed))
                return;

            var dayCalculator = new DayCalculator(organization, settings, employee, employmentPolicy);

            var timeEntries = new List<TimeEntry>();
            var regularHolidaysList = new List<DateTime>(); // Used for postlegalholidaycheck
            foreach (var currentDate in CalendarHelper.EachDay(_cutoffStart, _cutoffEnd))
            {
                try
                {
                    var timelog = timeLogs.OrderByDescending(t => t.LastUpd).FirstOrDefault(t => t.LogDate == currentDate);
                    var employeeShift = shiftSchedules.FirstOrDefault(s => s.EffectiveFrom <= currentDate && currentDate <= s.EffectiveTo);
                    var overtimes = overtimesInCutoff.Where(o => o.OTStartDate <= currentDate && currentDate <= o.OTEndDate).ToList();
                    var leaves = leavesInCutoff.Where(l => l.StartDate == currentDate).ToList();
                    var officialBusiness = officialBusinesses.FirstOrDefault(o => o.StartDate.Value == currentDate);
                    var dutyShiftSched = dutyShiftSchedules.FirstOrDefault(es => es.DateSched == currentDate);
                    var currentTimeAttendanceLogs = timeAttendanceLogs.Where(l => l.WorkDay == currentDate).ToList();

                    var branchId = timelog?.BranchID ?? employee?.BranchID;
                    var payrate = calendarCollection.GetCalendar(branchId).Find(currentDate);

                    var timeEntry = dayCalculator.Compute(
                        currentDate,
                        salary,
                        previousTimeEntries,
                        employeeShift,
                        dutyShiftSched,
                        timelog,
                        overtimes,
                        officialBusiness,
                        leaves,
                        currentTimeAttendanceLogs,
                        breakTimeBrackets,
                        payrate,
                        calendarCollection,
                        branchId);

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
                    throw new Exception($"{currentDate} #{employee.EmployeeNo}", ex);
                }
            }

            PostLegalHolidayCheck(employee, timeEntries, timeEntryPolicy, regularHolidaysList, calendarCollection);
            timeEntries.ForEach(t =>
            {
                t.RegularHolidayPay += t.BasicRegularHolidayPay;
                t.TotalDayPay += t.BasicRegularHolidayPay;
            });

            if (employee.IsUnderAgency)
            {
                var agency = agencies.SingleOrDefault(a => a.RowID == employee.AgencyID);

                var agencyCalculator = new AgencyFeeCalculator(employee, agency, agencyFees);
                agencyFees = agencyCalculator.Compute(timeEntries);
            }

            var actualTimeEntryCalculator = new ActualTimeEntryCalculator(salary, actualTimeEntries, new ActualTimeEntryPolicy(settings));
            actualTimeEntries = actualTimeEntryCalculator.Compute(timeEntries);

            using (var context = new PayrollContext(_dbContextOptionsService.DbContextOptions))
            {
                AddTimeEntriesToContext(userId, context, timeEntries);
                AddActualTimeEntriesToContext(userId, context, actualTimeEntries);
                AddAgencyFeesToContext(context, agencyFees);
                context.SaveChanges();
            }
        }

        private void PostLegalHolidayCheck(Employee employee,
                                            List<TimeEntry> timeEntries,
                                            TimeEntryPolicy timeEntryPolicy,
                                            List<DateTime> regularHolidaysList,
                                            CalendarCollection calendarCollection)
        {
            if (timeEntryPolicy.PostLegalHolidayCheck)
            {
                if (!employee.CalcHoliday)
                    return;

                if (regularHolidaysList.Any())
                {
                    foreach (var holidayDate in regularHolidaysList)
                    {
                        var presentAfterLegalHoliday = PayrollTools.HasWorkAfterLegalHoliday(
                            holidayDate,
                            _cutoffEnd,
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

        private void AddTimeEntriesToContext(int userId, PayrollContext context, IList<TimeEntry> timeEntries)
        {
            foreach (var timeEntry in timeEntries)
            {
                if (timeEntry.RowID.HasValue)
                {
                    timeEntry.LastUpdBy = userId;
                    context.Entry(timeEntry).State = EntityState.Modified;
                }
                else
                {
                    timeEntry.CreatedBy = userId;
                    context.TimeEntries.Add(timeEntry);
                }
            }
        }

        private void AddActualTimeEntriesToContext(int userId, PayrollContext context, ICollection<ActualTimeEntry> actualTimeEntries)
        {
            foreach (var actualTimeEntry in actualTimeEntries)
            {
                if (actualTimeEntry.RowID.HasValue)
                    context.Entry(actualTimeEntry).State = EntityState.Modified;
                else
                    context.ActualTimeEntries.Add(actualTimeEntry);
            }
        }

        private void AddAgencyFeesToContext(PayrollContext context, ICollection<AgencyFee> agencyFees)
        {
            foreach (var agencyFee in agencyFees)
            {
                if (agencyFee.RowID.HasValue)
                    context.Entry(agencyFee).State = EntityState.Modified;
                else
                    context.AgencyFees.Add(agencyFee);
            }
        }
    }
}