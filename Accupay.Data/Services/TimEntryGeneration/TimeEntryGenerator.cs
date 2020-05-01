using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using log4net;
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

        private readonly int _organizationId;
        private readonly DateTime _cutoffStart;
        private readonly DateTime _cutoffEnd;
        private readonly int _threeDays = 3;
        private IList<TimeEntry> _timeEntries;
        private IList<ActualTimeEntry> _actualTimeEntries;
        private IList<TimeLog> _timeLogs;
        private IList<Overtime> _overtimes;
        private IList<Leave> _leaves;
        private IList<OfficialBusiness> _officialBusinesses;
        private IList<AgencyFee> _agencyFees;
        private IList<ShiftSchedule> _employeeShifts;
        private IList<Salary> _salaries;
        private IList<EmployeeDutySchedule> _shiftSchedules;
        private List<TimeAttendanceLog> _timeAttendanceLogs;
        private List<BreakTimeBracket> _breakTimeBrackets;

        private AgencyRepository _agencyRepository;
        private BreakTimeBracketRepository _breakTimeBracketRepository;
        private EmployeeRepository _employeeRepository;
        private EmployeeDutyScheduleRepository _employeeDutyScheduleRepository;
        private LeaveRepository _leaveRepository;
        private OfficialBusinessRepository _officialBusinessRepository;
        private OrganizationRepository _organizationRepository;
        private OvertimeRepository _overtimeRepository;
        private SalaryRepository _salaryRepository;
        private ShiftScheduleRepository _shiftScheduleRepository;
        private TimeLogRepository _timeLogRepository;
        private TimeAttendanceLogRepository _timeAttendanceLogRepository;

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

                return Convert.ToInt32(Math.Floor(_finished / (double)_total * 100));
            }
        }

        public TimeEntryGenerator(int organizationId, DateTime cutoffStart, DateTime cutoffEnd)
        {
            _organizationId = organizationId;
            _cutoffStart = cutoffStart;
            _cutoffEnd = cutoffEnd;

            _agencyRepository = new AgencyRepository();
            _breakTimeBracketRepository = new BreakTimeBracketRepository();
            _employeeRepository = new EmployeeRepository();
            _employeeDutyScheduleRepository = new EmployeeDutyScheduleRepository();
            _leaveRepository = new LeaveRepository();
            _officialBusinessRepository = new OfficialBusinessRepository();
            _organizationRepository = new OrganizationRepository();
            _overtimeRepository = new OvertimeRepository();
            _salaryRepository = new SalaryRepository();
            _shiftScheduleRepository = new ShiftScheduleRepository();
            _timeAttendanceLogRepository = new TimeAttendanceLogRepository();
            _timeLogRepository = new TimeLogRepository();
        }

        public void Start()
        {
            IList<Employee> employees = null;
            Organization organization = null;
            IList<Agency> agencies = null;
            CalendarCollection calendarCollection;

            ListOfValueCollection settings = ListOfValueCollection.Create();
            TimeEntryPolicy timeEntryPolicy = new TimeEntryPolicy(settings);

            TimePeriod cuttOffPeriod = new TimePeriod(_cutoffStart, _cutoffEnd);

            using (var context = new PayrollContext())
            {
                employees = _employeeRepository.GetAllActiveWithPosition(_organizationId).ToList();

                agencies = _agencyRepository.GetAll(_organizationId).ToList();

                organization = _organizationRepository.GetById(_organizationId);

                _salaries = _salaryRepository.GetByCutOff(_organizationId, _cutoffStart).ToList();

                var previousCutoff = PayrollTools.GetPreviousCutoffDateForCheckingLastWorkingDay(_cutoffStart);

                DateTime endOfCutOff = _cutoffEnd;
                if (timeEntryPolicy.PostLegalHolidayCheck)
                {
                    endOfCutOff = _cutoffEnd.AddDays(_threeDays);
                }

                // TODO: move this to a repository
                _timeEntries = context.TimeEntries.
                                    Where(t => t.OrganizationID.Value == _organizationId).
                                    Where(t => previousCutoff <= t.Date && t.Date <= endOfCutOff).
                                    ToList();

                // TODO: move this to a repository
                _actualTimeEntries = context.ActualTimeEntries.
                                    Where(a => a.OrganizationID.Value == _organizationId).
                                    Where(a => _cutoffStart <= a.Date && a.Date <= _cutoffEnd).
                                    ToList();

                _timeLogs = _timeLogRepository.
                                GetByDatePeriod(_organizationId, cuttOffPeriod).
                                ToList();

                _leaves = _leaveRepository.
                                GetAllApprovedByDatePeriod(_organizationId, cuttOffPeriod).
                                ToList();

                _overtimes = _overtimeRepository.
                                GetByDatePeriod(_organizationId, cuttOffPeriod, OvertimeStatus.Approved).
                                ToList();

                _officialBusinesses = _officialBusinessRepository.
                                GetAllApprovedByDatePeriod(_organizationId, cuttOffPeriod).
                                ToList();

                // TODO: move this to a repository
                _agencyFees = context.AgencyFees.
                                Where(a => a.OrganizationID.Value == _organizationId).
                                Where(a => _cutoffStart <= a.Date && a.Date <= _cutoffEnd).
                                ToList();

                _employeeShifts = _shiftScheduleRepository.
                                GetByDatePeriod(_organizationId, cuttOffPeriod).
                                ToList();

                _shiftSchedules = _employeeDutyScheduleRepository.
                                GetByDatePeriod(_organizationId, cuttOffPeriod).
                                ToList();

                if (timeEntryPolicy.ComputeBreakTimeLate)
                {
                    _timeAttendanceLogs = _timeAttendanceLogRepository.
                                            GetByTimePeriod(_organizationId, cuttOffPeriod).
                                            ToList();

                    _breakTimeBrackets = _breakTimeBracketRepository.
                                            GetAll(_organizationId).
                                            ToList();
                }
                else
                {
                    _timeAttendanceLogs = new List<TimeAttendanceLog>();
                    _breakTimeBrackets = new List<BreakTimeBracket>();
                }

                var payrateCalculationBasis = settings.GetEnum("Pay rate.CalculationBasis", PayRateCalculationBasis.Organization);

                calendarCollection = PayrollTools.GetCalendarCollection(
                                                        new TimePeriod(previousCutoff, _cutoffEnd),
                                                        payrateCalculationBasis,
                                                        _organizationId);
            }

            var progress = new ObservableCollection<int>();

            _total = employees.Count;

            Parallel.ForEach(employees, employee =>
            {
                try
                {
                    CalculateEmployeeEntries(employee, organization, settings, agencies, timeEntryPolicy, calendarCollection);
                }
                catch (Exception)
                {
                    // can error if employee type is null
                    //logger.Error(ex.Message, ex);
                    _errors += 1;
                }

                Interlocked.Increment(ref _finished);
            });
        }

        private void CalculateEmployeeEntries(Employee employee,
                                            Organization organization,
                                            ListOfValueCollection settings,
                                            IList<Agency> agencies,
                                            TimeEntryPolicy timeEntryPolicy,
                                            CalendarCollection calendarCollection)
        {
            IList<TimeEntry> previousTimeEntries = _timeEntries.
                                                        Where(t => t.EmployeeID == employee.RowID).
                                                        ToList();

            IList<ActualTimeEntry> actualTimeEntries = _actualTimeEntries.
                                                        Where(a => a.EmployeeID == employee.RowID).
                                                        ToList();

            var salary = _salaries.FirstOrDefault(s => s.EmployeeID == employee.RowID);

            IList<TimeLog> timeLogs = _timeLogs.
                                        Where(t => t.EmployeeID == employee.RowID).
                                        ToList();

            IList<ShiftSchedule> shiftSchedules = _employeeShifts.
                                        Where(s => s.EmployeeID == employee.RowID).
                                        ToList();

            IList<Overtime> overtimesInCutoff = _overtimes.
                                        Where(o => o.EmployeeID == employee.RowID).
                                        ToList();

            IList<OfficialBusiness> officialBusinesses = _officialBusinesses.
                                        Where(o => o.EmployeeID == employee.RowID).
                                        ToList();

            IList<Leave> leavesInCutoff = _leaves.Where(l => l.EmployeeID == employee.RowID).
                                        Where(l => l.LeaveType != "Leave w/o Pay").
                                        ToList();

            IList<AgencyFee> agencyFees = _agencyFees.
                                        Where(a => a.EmployeeID == employee.RowID).
                                        ToList();

            var dutyShiftSchedules = _shiftSchedules.
                                        Where(es => es.EmployeeID == employee.RowID).
                                        ToList();

            IList<TimeAttendanceLog> timeAttendanceLogs = _timeAttendanceLogs.
                                        Where(t => t.EmployeeID == employee.RowID).
                                        ToList();

            IList<BreakTimeBracket> breakTimeBrackets = _breakTimeBrackets.
                                        Where(b => b.DivisionID == employee.Position?.DivisionID).
                                        ToList();

            if (employee.IsActive == false)
            {
                var currentTimeEntries = previousTimeEntries.
                                        Where(t => _cutoffStart <= t.Date && t.Date <= _cutoffEnd);

                // TODO: return this as one the list of errors of Time entry generation
                if (!currentTimeEntries.Any())
                    return;
            }

            // TODO: return this as one the list of errors of Time entry generation
            if (!(timeLogs.Any() || leavesInCutoff.Any() || officialBusinesses.Any()) && (!employee.IsFixed))
                return;

            var dayCalculator = new DayCalculator(organization, settings, employee);

            var timeEntries = new List<TimeEntry>();
            var regularHolidaysList = new List<DateTime>(); // Used for postlegalholidaycheck
            foreach (var currentDate in CalendarHelper.EachDay(_cutoffStart, _cutoffEnd))
            {
                try
                {
                    var timelog = timeLogs.OrderByDescending(t => t.LastUpd).FirstOrDefault(t => t.LogDate == currentDate);
                    var employeeShift = shiftSchedules.FirstOrDefault(s => s.EffectiveFrom <= currentDate & currentDate <= s.EffectiveTo);
                    var overtimes = overtimesInCutoff.Where(o => o.OTStartDate <= currentDate & currentDate <= o.OTEndDate).ToList();
                    var leaves = leavesInCutoff.Where(l => l.StartDate == currentDate).ToList();
                    var officialBusiness = officialBusinesses.FirstOrDefault(o => o.StartDate.Value == currentDate);
                    var dutyShiftSched = dutyShiftSchedules.FirstOrDefault(es => es.DateSched == currentDate);
                    var currentTimeAttendanceLogs = timeAttendanceLogs.Where(l => l.WorkDay == currentDate).ToList();

                    var branchId = timelog?.BranchID ?? employee?.BranchID;
                    var payrate = calendarCollection.GetCalendar(branchId).Find(currentDate);

                    var timeEntry = dayCalculator.Compute(currentDate,
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

            using (var context = new PayrollContext())
            {
                AddTimeEntriesToContext(context, timeEntries);
                AddActualTimeEntriesToContext(context, actualTimeEntries);
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
                        var presentAfterLegalHoliday = PayrollTools.
                                                        HasWorkAfterLegalHoliday(holidayDate,
                                                                                _cutoffEnd,
                                                                                timeEntries,
                                                                                calendarCollection);

                        if (!presentAfterLegalHoliday)
                        {
                            var timeEntry = timeEntries.
                                        Where(t => t.Date == holidayDate).
                                        FirstOrDefault();

                            timeEntry.BasicRegularHolidayPay = 0;
                        }
                    }
                }
            }
        }

        private void AddTimeEntriesToContext(PayrollContext context, IList<TimeEntry> timeEntries)
        {
            foreach (var timeEntry in timeEntries)
            {
                if (timeEntry.RowID.HasValue)
                    context.Entry(timeEntry).State = EntityState.Modified;
                else
                    context.TimeEntries.Add(timeEntry);
            }
        }

        private void AddActualTimeEntriesToContext(PayrollContext context, IList<ActualTimeEntry> actualTimeEntries)
        {
            foreach (var actualTimeEntry in actualTimeEntries)
            {
                if (actualTimeEntry.RowID.HasValue)
                    context.Entry(actualTimeEntry).State = EntityState.Modified;
                else
                    context.ActualTimeEntries.Add(actualTimeEntry);
            }
        }

        private void AddAgencyFeesToContext(PayrollContext context, IList<AgencyFee> agencyFees)
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