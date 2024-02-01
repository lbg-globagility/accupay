using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports;
using AccuPay.Core.ValueObjects;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class TimeLogImportParser : ITimeLogImportParser
    {
        private readonly ITimeLogsReader _reader;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IOvertimeRepository _overtimeRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IShiftRepository _shiftRepository;

        public TimeLogImportParser(
            ITimeLogsReader reader,
            IEmployeeRepository employeeRepository,
            IOvertimeRepository overtimeRepository,
            IShiftRepository shiftRepository,
            ITimeEntryRepository timeEntryRepository)
        {
            _reader = reader;
            _employeeRepository = employeeRepository;
            _shiftRepository = shiftRepository;
            _overtimeRepository = overtimeRepository;
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<TimeLogImportParserOutput> Parse(string importFile, int organizationId, int userId)
        {
            var parsedRecords = _reader.Read(importFile);
            if (!parsedRecords.Logs.Any() && !parsedRecords.Errors.Any())
                throw new BusinessLogicException("No logs were parsed. Please make sure the log files follows the right format.");
            else if (!parsedRecords.Logs.Any())
            {
                AllParsedLogOutput errorParsedLogs = new AllParsedLogOutput(parsedRecords.Logs, parsedRecords.Errors);
                return new TimeLogImportParserOutput(errorParsedLogs, new List<TimeLog>(), null);
            }

            var helper = await GetHelper(parsedRecords.Logs.ToList(), organizationId: organizationId, userId: userId);

            AllParsedLogOutput allParsedLogs = GetAllLogs(helper, parsedRecords.Errors);

            var timeLogs = helper.GenerateTimeLogs();
            var timeAttendanceLogs = helper.GenerateTimeAttendanceLogs();

            return new TimeLogImportParserOutput(allParsedLogs, timeLogs, timeAttendanceLogs);
        }

        public AllParsedLogOutput GetAllLogs(ITimeAttendanceHelper helper, IList<TimeLogImportModel> parsedErrors)
        {
            // determines the IstimeIn, LogDate, and Employee values
            var allLogs = helper.Analyze();

            var validLogs = allLogs.Where(x => !x.HasError).ToList();
            var invalidLogs = allLogs.Where(x => x.HasError).ToList();

            invalidLogs.AddRange(parsedErrors);

            validLogs = validLogs
                .OrderBy(x => x.Employee?.LastName)
                .ThenBy(x => x.Employee?.FirstName)
                .ThenBy(x => x.Employee?.MiddleName)
                .ThenBy(x => x.LogDate)
                .ThenBy(x => x.DateTime)
                .ToList();

            invalidLogs = invalidLogs
                .OrderBy(l => l.LineNumber)
                .ToList();

            return new AllParsedLogOutput(validRecords: validLogs, invalidRecords: invalidLogs);
        }

        public async Task<ITimeAttendanceHelper> GetHelper(List<TimeLogImportModel> logs, int organizationId, int userId)
        {
            logs = logs.OrderBy(l => l.DateTime).ToList();

            var logsGroupedByEmployee = TimeLogImportModel.GroupByEmployee(logs);
            List<Employee> employees = await GetEmployeesFromLogGroup(logsGroupedByEmployee, organizationId);

            var firstDate = logs.FirstOrDefault().DateTime.ToMinimumHourValue();
            var lastDate = logs.LastOrDefault().DateTime.ToMaximumHourValue();

            ITimeAttendanceHelper timeAttendanceHelper;

            TimePeriod datePeriod = new TimePeriod(firstDate, lastDate);

            List<Shift> employeeShifts = await GetEmployeeShifts(datePeriod, organizationId);

            List<Overtime> employeeOvertimes = await GetEmployeeOvertime(datePeriod, organizationId);

            var timeEntryPolicy = await _timeEntryRepository.GetTimeEntryPolicy();

            timeAttendanceHelper = new TimeAttendanceHelper(logs, employees, employeeShifts, employeeOvertimes, organizationId: organizationId, userId: userId, timeEntryPolicy: timeEntryPolicy);

            return timeAttendanceHelper;
        }

        private async Task<List<Employee>> GetEmployeesFromLogGroup(List<IGrouping<string, TimeLogImportModel>> logsGroupedByEmployee, int organizationId)
        {
            if (logsGroupedByEmployee.Count < 1)
                return new List<Employee>();

            var employeeNumbersArray = logsGroupedByEmployee.Select(x => x.Key).Distinct().ToArray();

            var employees = await _employeeRepository.GetByMultipleEmployeeNumberAsync(employeeNumbersArray, organizationId);

            return employees.ToList();
        }

        private async Task<List<Shift>> GetEmployeeShifts(TimePeriod timePeriod, int organizationId)
        {
            return (await _shiftRepository.GetByDatePeriodAsync(organizationId, timePeriod)).ToList();
        }

        private async Task<List<Overtime>> GetEmployeeOvertime(TimePeriod timePeriod, int organizationId)
        {
            return (await _overtimeRepository.GetByDatePeriodAsync(organizationId, timePeriod)).ToList();
        }

        public class AllParsedLogOutput
        {
            public IList<TimeLogImportModel> ValidRecords { get; }

            public IList<TimeLogImportModel> InvalidRecords { get; }

            public AllParsedLogOutput(
                IList<TimeLogImportModel> validRecords,
                IList<TimeLogImportModel> invalidRecords)
            {
                ValidRecords = validRecords;
                InvalidRecords = invalidRecords;
            }
        }

        public class TimeLogImportParserOutput
        {
            public IReadOnlyCollection<TimeLogImportModel> ValidRecords { get; }

            public IReadOnlyCollection<TimeLogImportModel> InvalidRecords { get; }

            public IReadOnlyCollection<TimeLog> GeneratedTimeLogs { get; }

            public IReadOnlyCollection<TimeAttendanceLog> GeneratedTimeAttendanceLogs { get; }

            public TimeLogImportParserOutput(
                AllParsedLogOutput parsedLogs,
                IReadOnlyCollection<TimeLog> timeLogs,
                IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs)
            {
                ValidRecords = parsedLogs.ValidRecords.ToList();
                InvalidRecords = parsedLogs.InvalidRecords.ToList();

                GeneratedTimeLogs = timeLogs;
                GeneratedTimeAttendanceLogs = timeAttendanceLogs;
            }
        }
    }
}
