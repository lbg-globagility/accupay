using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services.Imports;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class TimeAttendanceHelperNew : ITimeAttendanceHelper
    {
        private enum OvertimeType
        {
            Nothing,
            Pre,
            Post
        }

        private const double HOURS_BUFFER = 4;

        private List<TimeLogImportModel> _importedTimeAttendanceLogs;
        private List<IGrouping<string, TimeLogImportModel>> _logsGroupedByEmployee;

        private readonly List<Employee> _employees;
        private readonly List<Overtime> _employeeOvertimes;
        private readonly List<EmployeeDutySchedule> _employeeShifts;

        private readonly int _organizationId;
        private readonly int _userId;

        public TimeAttendanceHelperNew(
            List<TimeLogImportModel> importedTimeLogs,
            List<Employee> employees,
            List<EmployeeDutySchedule> employeeShifts,
            List<Overtime> employeeOvertimes,
            int organizationId,
            int userId)
        {
            _importedTimeAttendanceLogs = importedTimeLogs;
            _employees = employees;
            _employeeShifts = employeeShifts;
            _employeeOvertimes = employeeOvertimes;
            _organizationId = organizationId;
            _userId = userId;

            _logsGroupedByEmployee = TimeLogImportModel
                .GroupByEmployee(_importedTimeAttendanceLogs);

            GetEmployeeObjectOfLogs();
        }

        public List<TimeLogImportModel> Analyze()
        {
            var dayLogRecords = GenerateDayLogRecords();

            foreach (var dayLogRecord in dayLogRecords)
            {
                var timeAttendanceLogs = dayLogRecord.LogRecords
                    .Where(d => d.IsTimeIn == null)
                    .OrderBy(d => d.DateTime)
                    .ToList();

                var index = 0;
                var lastIndex = timeAttendanceLogs.Count - 1;

                var isPreviouslyTimeIn = false;

                foreach (var timeAttendanceLog in timeAttendanceLogs)
                {
                    timeAttendanceLog.IsTimeIn = null;

                    if (index == 0)
                        timeAttendanceLog.IsTimeIn = true;
                    else if (index == lastIndex)
                        timeAttendanceLog.IsTimeIn = false;
                    else
                        timeAttendanceLog.IsTimeIn = !isPreviouslyTimeIn;

                    isPreviouslyTimeIn = timeAttendanceLog.IsTimeIn ?? false;

                    timeAttendanceLog.LogDate = dayLogRecord.LogDate;

                    timeAttendanceLog.ShiftTimeInBounds = dayLogRecord.ShiftTimeInBounds;

                    timeAttendanceLog.ShiftTimeOutBounds = dayLogRecord.ShiftTimeOutBounds;

                    timeAttendanceLog.EmployeeDutySchedule = dayLogRecord.ShiftSchedule;

                    index += 1;
                }
            }

            return _importedTimeAttendanceLogs;
        }

        public List<TimeLogImportModel> Validate()
        {
            // #1 Convert it back to DayLogRecords
            List<DayLogRecord> dayLogRecords = new List<DayLogRecord>();

            foreach (var logGroup in _logsGroupedByEmployee)
            {
                if (logGroup.Count() == 0) continue;

                var employeeId = logGroup.FirstOrDefault().Employee?.RowID;

                if (employeeId == null) continue;

                var employeeLogs = logGroup.ToList();

                var earliestLog = logGroup.FirstOrDefault().DateTime.ToMinimumHourValue();
                var earliestLogDate = logGroup.FirstOrDefault().LogDate.ToMinimumHourValue();
                var lastlog = logGroup.LastOrDefault().DateTime.ToMaximumHourValue();
                var lastLogDate = logGroup.LastOrDefault().LogDate.ToMaximumHourValue();

                var earliestDate = new[] { earliestLog, earliestLogDate }.Min();
                var lastDate = new[] { lastlog, lastLogDate }.Max();

                if (earliestDate.HasValue == false || lastDate.HasValue == false)
                    continue;

                foreach (var currentDate in CalendarHelper.EachDay(earliestDate.Value, lastDate.Value))
                {
                    var currentEmployeeLogs = employeeLogs
                        .Where(l => l.LogDate == currentDate)
                        .Where(l => l.HasError == false)
                        .ToList();

                    if (currentEmployeeLogs.Any() == false)
                        continue;

                    foreach (var log in currentEmployeeLogs)

                        log.WarningMessage = null;

                    dayLogRecords.Add(new DayLogRecord()
                    {
                        EmployeeId = employeeId.Value,
                        LogDate = currentDate,
                        LogRecords = currentEmployeeLogs.OrderBy(l => l.DateTime).ToList()
                    });
                }
            }

            // #2 revalidate the logs. '[Check for succeeding logs that are both INs Or OUTs]
            ValidateLogs(dayLogRecords);

            return _importedTimeAttendanceLogs;
        }

        public List<TimeLog> GenerateTimeLogs()
        {
            List<TimeLog> timeLogs = new List<TimeLog>();

            foreach (var logGroups in _logsGroupedByEmployee)
            {
                var timeAttendanceLogOfEmployee = logGroups.ToList();

                if (timeAttendanceLogOfEmployee.Count == 0)
                    continue;

                var currentEmployee = timeAttendanceLogOfEmployee[0].Employee;

                if (currentEmployee == null)
                    continue;

                var logsByDayGroup = timeAttendanceLogOfEmployee.GroupBy(l => l.LogDate).ToList();

                foreach (var logsByDay in logsByDayGroup)
                {
                    var logsByDayList = logsByDay
                        .Where(l => l.IsTimeIn.HasValue)
                        .OrderBy(x => x.DateTime)
                        .ToList();

                    if (logsByDay.Key == null)
                        continue;

                    var logDate = Convert.ToDateTime(logsByDay.Key);

                    var firstTimeStampIn = logsByDayList
                        .FirstOrDefault(l => l.IsTimeIn.Value == true)?
                        .DateTime;
                    var finalTimeStampOut = logsByDayList
                        .LastOrDefault(l => l.IsTimeIn.Value == false)?
                        .DateTime;

                    TimeSpan? firstTimeIn, finalTimeOut;

                    // using timestampin with timeOfDay does not result to nothing
                    if (firstTimeStampIn == null)
                        firstTimeIn = null;
                    else
                        firstTimeIn = firstTimeStampIn.Value.TimeOfDay;
                    if (finalTimeStampOut == null)
                        finalTimeOut = null;
                    else
                        finalTimeOut = finalTimeStampOut.Value.TimeOfDay;

                    timeLogs.Add(new TimeLog()
                    {
                        LogDate = logDate,
                        CreatedBy = _userId,
                        OrganizationID = _organizationId,
                        Employee = currentEmployee,
                        EmployeeID = currentEmployee.RowID,
                        TimeIn = firstTimeIn,
                        TimeOut = finalTimeOut,
                        TimeStampIn = firstTimeStampIn,
                        TimeStampOut = finalTimeStampOut,
                        BranchID = currentEmployee.BranchID,
                        Branch = currentEmployee.Branch
                    });
                }
            }

            return timeLogs;
        }

        public List<TimeAttendanceLog> GenerateTimeAttendanceLogs()
        {
            List<TimeAttendanceLog> timeAttendanceLogs = new List<TimeAttendanceLog>();

            foreach (var log in _importedTimeAttendanceLogs)
            {
                if (log.LogDate == null)
                    continue;

                var employeeId = log.Employee?.RowID;

                if (employeeId == null)
                    continue;

                timeAttendanceLogs.Add(new TimeAttendanceLog()
                {
                    CreatedBy = _userId,
                    OrganizationID = _organizationId,
                    TimeStamp = log.DateTime,
                    IsTimeIn = log.IsTimeIn,
                    WorkDay = Convert.ToDateTime(log.LogDate),
                    EmployeeID = Convert.ToInt32(employeeId)
                });
            }

            return timeAttendanceLogs;
        }

        private List<DayLogRecord> GenerateDayLogRecords()
        {
            List<DayLogRecord> dayLogRecords = new List<DayLogRecord>();

            foreach (var logGroup in _logsGroupedByEmployee)
            {
                if (logGroup.Count() == 0)
                    continue;

                var employeeId = logGroup.FirstOrDefault().Employee?.RowID;

                if (employeeId == null)
                    continue;

                var currentEmployeeShifts = _employeeShifts
                    .Where(s => s.EmployeeID == employeeId)
                    .ToList();

                var currentEmployeeOvertimes = _employeeOvertimes
                    .Where(o => o.EmployeeID == employeeId)
                    .ToList();

                var employeeLogs = logGroup.ToList();

                var earliestDate = logGroup.FirstOrDefault().DateTime.Date.ToMinimumHourValue();
                var lastDate = logGroup.LastOrDefault().DateTime.Date.ToMaximumHourValue();

                DayLogRecord lastDayLogRecord = null;

                foreach (var currentDate in CalendarHelper.EachDay(earliestDate, lastDate))
                {
                    var dayLogRecord = GenerateDayLogRecord(employeeId.Value, employeeLogs, currentEmployeeShifts, currentEmployeeOvertimes, currentDate, lastDayLogRecord);

                    if (dayLogRecord != null)
                    {
                        dayLogRecords.Add(dayLogRecord);
                        lastDayLogRecord = dayLogRecord;
                    }
                    else
                        lastDayLogRecord = null;
                }
            }

            return dayLogRecords;
        }

        private DayLogRecord GenerateDayLogRecord(int employeeId, List<TimeLogImportModel> employeeLogs, List<EmployeeDutySchedule> currentEmployeeShifts, List<Overtime> currentEmployeeOvertimes, DateTime currentDate, DayLogRecord lastDayLogRecord)
        {
            var currentShift = GetShift(currentEmployeeShifts, currentDate);

            var nextDate = currentDate.AddDays(1);
            var nextShift = GetShift(currentEmployeeShifts, nextDate);

            var earliestOvertime = GetEarliestOvertime(currentEmployeeOvertimes, currentDate, currentShift);

            var lastOvertime = GetLastOvertime(currentEmployeeOvertimes, currentDate, currentShift);

            var earliestOvertimeNextDay = GetEarliestOvertime(currentEmployeeOvertimes, nextDate, nextShift);

            var timeInBounds = GetShiftBoundsForTimeIn(lastDayLogRecord?.ShiftTimeOutBounds, currentDate, currentShift, earliestOvertime);

            var timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, lastOvertime, nextShift, earliestOvertimeNextDay);

            var dayBounds = new TimePeriod(timeInBounds, timeOutBounds);

            return new DayLogRecord()
            {
                EmployeeId = employeeId,
                LogDate = currentDate,
                LogRecords = GetTimeLogsFromBounds(dayBounds, employeeLogs, lastDayLogRecord),
                ShiftTimeInBounds = dayBounds.Start,
                ShiftTimeOutBounds = dayBounds.End,
                ShiftSchedule = currentShift
            };
        }

        /// <summary>
        ///     ''' Check if the OT is really a pre OT or a .
        ///     ''' </summary>
        ///     ''' <param name="overtime"></param>
        ///     ''' <param name="shift"></param>
        ///     ''' <returns></returns>
        private OvertimeType CheckOvertimeType(Overtime overtime, EmployeeDutySchedule shift)
        {
            // Sometimes the OT StartTime is less than the shift StartTime
            // but it can be a post OT.

            // FOR NIGHT SHIFT
            // for post shift
            // Example 1:   7PM-4AM shift / 4:30AM - 8:30AM OT
            // OT Is Post OT

            // For pre shift
            // (Example 2:   7PM-4AM shift / 2:30PM - 6:30PM OT
            // OT Is Pre OT)

            // FOR DAY SHIFT
            // for post shift
            // Example 1:   9AM-6PM shift / 6:30PM - 10:30PM OT
            // OT Is Post OT

            // For pre shift
            // (Example 2:  9AM-6PM shift / 4:30AM - 8:30AM OT
            // OT Is Pre OT)

            if (shift == null || shift.StartTime == null || shift.EndTime == null || overtime == null || overtime.OTStartTime == null || overtime.OTEndTime == null)
                return OvertimeType.Nothing;

            // ex1: 08:30AM to 7PM = 11.5 hours
            // ex2: 10:30PM to 9AM = 10.5 hours (negative)
            var preOvertimeBreakSpan = ComputeHoursDifference(overtime.OTEndTime.Value, shift.StartTime.Value);

            // ex1: 4AM to 4:30AM = 0.5 hours
            // ex2: 6PM to 6:30PM = 0.5 hours
            var postOvertimeBreakSpan = ComputeHoursDifference(shift.EndTime.Value, overtime.OTStartTime.Value);

            // check where is the OT closest. Is it on the end shift time or start shift time
            if (preOvertimeBreakSpan < postOvertimeBreakSpan)
                return OvertimeType.Pre;
            else
                // prioritize Post OT that is why it is the one on the else block
                return OvertimeType.Post;
        }

        private decimal ComputeHoursDifference(TimeSpan start, TimeSpan end)
        {
            // No difference
            if (start == end)
                return 0;

            return TimePeriod.FromTime(start, end, DateTime.Now.ToMinimumHourValue()).TotalHours;
        }

        private EmployeeDutySchedule GetShift(List<EmployeeDutySchedule> currentEmployeeShifts, DateTime currentDate)
        {
            return currentEmployeeShifts.Where(s => s.DateSched == currentDate).FirstOrDefault();
        }

        private Overtime GetEarliestOvertime(List<Overtime> currentEmployeeOvertimes, DateTime currentDate, EmployeeDutySchedule currentShift)
        {
            return currentEmployeeOvertimes.Where(o =>
            {
                if (!IsTodaysOvertime(o, currentDate, currentShift))
                    return false;

                return CheckOvertimeType(o, currentShift) == OvertimeType.Pre;
            }).OrderBy(o => o.OTStartTime).FirstOrDefault();
        }

        private Overtime GetLastOvertime(List<Overtime> currentEmployeeOvertimes, DateTime currentDate, EmployeeDutySchedule currentShift)
        {
            return currentEmployeeOvertimes.Where(o =>
            {
                if (!IsTodaysOvertime(o, currentDate, currentShift))
                    return false;

                return CheckOvertimeType(o, currentShift) == OvertimeType.Post;
            }).OrderBy(o => o.OTStartTime).LastOrDefault();
        }

        /// <summary>
        ///     ''' Checks if overtime and currentShift is not null then check if OT Startdate is equal to currentDate.
        ///     ''' </summary>
        ///     ''' <param name="overtime"></param>
        ///     ''' <param name="currentDate"></param>
        ///     ''' <param name="currentShift"></param>
        ///     ''' <returns></returns>
        private bool IsTodaysOvertime(Overtime overtime, DateTime currentDate, EmployeeDutySchedule currentShift)
        {
            if (overtime == null ||
                currentShift == null ||
                overtime.OTStartDate != currentDate ||
                overtime.OTStartTime == null ||
                overtime.OTEndTime == null ||
                currentShift.StartTime == null ||
                currentShift.EndTime == null)
                return false;
            else
                return true;
        }

        private List<TimeLogImportModel> GetTimeLogsFromBounds(TimePeriod shiftBounds, List<TimeLogImportModel> timeAttendanceLogs, DayLogRecord lastDayLogRecord
    )
        {
            var logRecords = timeAttendanceLogs
                .Where(t => t.DateTime >= shiftBounds.Start)
                .Where(t => t.DateTime <= shiftBounds.End)
                .ToList();

            if (lastDayLogRecord?.ShiftTimeOutBounds != null && lastDayLogRecord?.ShiftTimeOutBounds == shiftBounds.Start && lastDayLogRecord?.LogRecords != null)
            {
                // if this happens, this maybe caused by an employee that after previous overtime, [A1]
                // he continued his current shift right after
                // ex: previous day OT end = 9:00 AM | current day shift start = 9:00 AM

                // if there are multiple logs in the previous day log record
                // that are the same as the start shift bounds
                // ex: multiple logs of 9:00 AM, maybe caused by employee
                // logging out for previous OT then logging in again for current shift
                // we need to get at least one same log from previous day log record
                // And transfer it to current logs
                var shiftBoundsStartLogs = lastDayLogRecord.LogRecords
                    .Where(l => l.DateTime == shiftBounds.Start)
                    .ToList();

                if (shiftBoundsStartLogs.Count > 1)
                {
                    // get 1 same log from previous day log record
                    var shiftBoundStartLog = shiftBoundsStartLogs[shiftBoundsStartLogs.Count - 1];

                    logRecords.Add(shiftBoundStartLog);

                    lastDayLogRecord?.LogRecords.Remove(shiftBoundStartLog);
                }
            }

            return logRecords;
        }

        private DateTime GetShiftBoundsForTimeIn(DateTime? previousTimeOutBounds, DateTime currentDate, EmployeeDutySchedule currentShift, Overtime earliestOvertime)
        {
            var currentDayStartDateTime = GetStartDateTime(currentShift, earliestOvertime);

            if (previousTimeOutBounds != null)
            {
                if (currentDayStartDateTime != null && currentDayStartDateTime == previousTimeOutBounds)
                {
                    // if this happens, this maybe caused by an employee that after previous overtime, [A1]
                    // he continued his current shift right after
                    // ex: previous day OT end = 9:00 AM | current day shift start = 9:00 AM

                    return previousTimeOutBounds.Value;
                }

                return previousTimeOutBounds.Value.AddSeconds(1);
            }

            DateTime shiftMinBound;

            if (currentDayStartDateTime != null)
            {
                // If merong shift or OT, minimum bound should be earliest shift or OT - HOURS_BUFFER (4 hours)
                // (ex. 9:00 AM - 5:00 PM shift -> 5:00 AM minimum bound)
                shiftMinBound = currentDayStartDateTime.Value.Add(TimeSpan.FromHours(-HOURS_BUFFER));
            }
            else
            {
                // If walang shift and OT, minimum bound should be 12:00 AM
                shiftMinBound = currentDate.ToMinimumHourValue();
            }

            return shiftMinBound;
        }

        private DateTime GetShiftBoundsForTimeOut(DateTime currentDate, EmployeeDutySchedule currentShift, Overtime lastOvertime, EmployeeDutySchedule nextShift, Overtime earliestOvertimeNextDay)
        {
            DateTime shiftMaxBound;

            double hoursBuffer = HOURS_BUFFER;

            DateTime? currentDayEndTime = GetEndDateTime(currentShift, lastOvertime);

            if (nextShift == null)
            {
                // if there is no next shift, disregard the next day overtime
                // scenario: should be next shift is (6am-3pm), OT is (3pm-4pm)
                // if no shift, the nextDayStartDateTime would be 3pm which would be wrong
                earliestOvertimeNextDay = null;
            }

            DateTime? nextDayStartDateTime = GetStartDateTime(nextShift, earliestOvertimeNextDay);

            if (currentDayEndTime == null && nextDayStartDateTime == null)
            {
                // no current day shift or overtime
                // and no next day shift or overtime
                // = maximum bound should be 11:59 PM
                shiftMaxBound = currentDate.ToMaximumHourValue();
            }
            else if (currentDayEndTime != null && nextDayStartDateTime != null)
            {
                // has current day shift or overtime
                // and has next day shift or overtime

                if (currentDayEndTime > nextDayStartDateTime)
                {
                    // this may happen when there is input error
                    // if current shift or late overtime end time is greater than
                    // next day shift or early overtime start time

                    shiftMaxBound = nextDayStartDateTime.Value.Add(TimeSpan.FromHours(-hoursBuffer)).Add(TimeSpan.FromSeconds(-1));
                }
                else if (currentDayEndTime == nextDayStartDateTime)
                {
                    // if this happens, this maybe caused by an employee that after overtime, [A1]
                    // he continued his next shift right after
                    // ex: current OT end = 9:00 AM | next day shift start = 9:00 AM

                    shiftMaxBound = nextDayStartDateTime.Value;
                }
                else
                {
                    // maximum bound should be halfway of the
                    // current day end time and next day start date
                    // ex 1: current day end time = 3:00 AM, next day start date = 9:00 AM (difference is 6 hours)
                    // = maximum bound should be 5:59 AM (next day -3 hours -1 second)
                    // if difference is greater HOURS_BUFFER * 2, use the default HOURS_BUFFER
                    // ex 2: current day end time = 10:00 PM, next day start date = 9:00 AM (difference is 11 hours)
                    // = maximum bound should be 4:59 AM (next day -4 hours -1 second)

                    var restPeriodHours = (double)new TimePeriod(currentDayEndTime.Value, nextDayStartDateTime.Value).TotalHours;

                    if (restPeriodHours < (HOURS_BUFFER * 2))
                        hoursBuffer = (double)AccuMath.CommercialRound(restPeriodHours / 2);

                    shiftMaxBound = nextDayStartDateTime.Value.Add(TimeSpan.FromHours(-hoursBuffer)).Add(TimeSpan.FromSeconds(-1));
                }
            }
            else if (currentDayEndTime != null)
            {
                // currentDayEndTime IsNot Nothing AndAlso nextDayStartDateTime Is Nothing

                shiftMaxBound = currentDayEndTime.ToMaximumHourValue().Value;
            }
            else
            {
                // nextDayStartDateTime IsNot Nothing AndAlso currentDayEndTime Is Nothing

                // if no next day shift or over time but has
                // current shift or overtime, set max bound time = currentDayEndTime - HOURS_BUFFER - 1 second
                // ex: next day start time is 9:00 AM - max bound time = 4:59 AM
                // ex: next day start end time is 5:00 AM - max bound time = 12:59 AM
                shiftMaxBound = nextDayStartDateTime.Value.Add(TimeSpan.FromHours(-HOURS_BUFFER)).Add(TimeSpan.FromSeconds(-1));
            }

            return shiftMaxBound;
        }

        private DateTime? GetStartDateTime(EmployeeDutySchedule currentShift, Overtime earliestOvertime)
        {
            var currentShiftStartTime = CreateDateTime(currentShift?.DateSched, currentShift?.StartTime);

            var earliestOvertimeStartTime = CreateDateTime(earliestOvertime?.OTStartDate, earliestOvertime?.OTStartTime);

            DateTime? startDateTime = CompareShiftToOvertime(currentShiftStartTime, earliestOvertimeStartTime, getEarliest: true);
            return startDateTime;
        }

        private DateTime? GetEndDateTime(EmployeeDutySchedule currentShift, Overtime lastOvertime)
        {
            var currentShiftEndTime = CreateDateTime(currentShift?.DateSched, currentShift?.EndTime, currentShift?.StartTime);
            DateTime? lastOvertimeEndTime = GetLastOvertime(currentShift, lastOvertime);

            DateTime? endTime = CompareShiftToOvertime(currentShiftEndTime, lastOvertimeEndTime, getEarliest: false);
            return endTime;
        }

        private DateTime? GetLastOvertime(EmployeeDutySchedule currentShift, Overtime lastOvertime)
        {
            var lastOvertimeEndTime = CreateDateTime(lastOvertime?.OTStartDate, lastOvertime?.OTEndTime, lastOvertime?.OTStartTime);

            // if OT start time < shift start time,
            // the OT is most likely a night shift Post OT
            if (currentShift?.StartTime != null && lastOvertime?.OTStartTime != null && lastOvertimeEndTime != null && lastOvertime?.OTStartTime < currentShift?.StartTime)
                lastOvertimeEndTime = lastOvertimeEndTime.Value.AddDays(1);

            return lastOvertimeEndTime;
        }

        private DateTime? CompareShiftToOvertime(DateTime? shiftDateTime, DateTime? overtimeDateTime, bool getEarliest)
        {
            if (overtimeDateTime == null && shiftDateTime == null)
                return null;
            else if (overtimeDateTime != null && shiftDateTime != null)
            {
                if (getEarliest)
                    return new DateTime[] { overtimeDateTime.Value, shiftDateTime.Value }.Min();
                else
                    return new DateTime[] { overtimeDateTime.Value, shiftDateTime.Value }.Max();
            }
            else if (overtimeDateTime != null)
                return overtimeDateTime.Value;
            else
                return shiftDateTime.Value;
        }

        private DateTime? CreateDateTime(DateTime? date, TimeSpan? time, TimeSpan? startTime = default(TimeSpan?))
        {
            if (date == null || time == null)
                return null;

            // for night shift, add 1 day
            if (startTime != null && startTime >= time)
                return date.Value.AddDays(1).ToMinimumHourValue().AddTicks(time.Value.Ticks);
            else
                return date.Value.ToMinimumHourValue().AddTicks(time.Value.Ticks);
        }

        private void GetEmployeeObjectOfLogs()
        {
            foreach (var logGroup in _logsGroupedByEmployee)
            {
                var employee = _employees.FirstOrDefault(e => e.EmployeeNo == logGroup.Key);

                foreach (var log in logGroup)
                {
                    if (log.HasError)
                        continue;

                    if (employee == null)
                    {
                        log.Employee = null;

                        log.ErrorMessage = "Employee not found in the database.";
                    }
                    else
                        log.Employee = employee;
                }
            }
        }

        private void ValidateLogs(List<DayLogRecord> dayLogRecords)
        {
            var succeedingInOrOutLogsWarningMessage = "This day has succeeding logs that are both IN or OUT.";

            foreach (var dayLogRecord in dayLogRecords)
            {
                // check if logs for the day was ok
                var logsWithTimeStatus = dayLogRecord.LogRecords
                    .Where(l => l.IsTimeIn != null)
                    .OrderBy(l => l.DateTime)
                    .ToList();

                string warningMessage = null;

                // if the total count for that day is even, it means it is ok
                // because it means its logs are alternating IN then OUT
                // if it is odd then there are succeeding logs that are either both IN or OUT
                if ((logsWithTimeStatus.Count % 2 != 0))
                {
                    warningMessage = succeedingInOrOutLogsWarningMessage;
                }
                else
                {
                    // Check if there are succeeding logs that are either both IN or OUT
                    // and it should start with Time In
                    var isTimeIn = true;

                    foreach (var log in logsWithTimeStatus)
                    {
                        if (log.IsTimeIn != isTimeIn)
                        {
                            warningMessage = succeedingInOrOutLogsWarningMessage;

                            break;
                        }

                        isTimeIn = !isTimeIn;
                    }
                }

                if (warningMessage != null)
                {
                    foreach (var log in logsWithTimeStatus)
                    {
                        log.WarningMessage = warningMessage;
                    }
                }
            }

            // add warning message for logs that were not including in a "day".
            var logsWithNoDateOrTimeStatus = _importedTimeAttendanceLogs
                .Where(l => l.LogDate == null)
                .Where(l => l.IsTimeIn == null)
                .ToList();

            foreach (var log in logsWithNoDateOrTimeStatus)
            {
                log.WarningMessage = "The analyzer were not able to figure out this log's date.";
            }
        }

        private class DayLogRecord
        {
            public int EmployeeId { get; set; }
            public DateTime LogDate { get; set; }
            public List<TimeLogImportModel> LogRecords { get; set; }
            public DateTime ShiftTimeInBounds { get; set; }
            public DateTime ShiftTimeOutBounds { get; set; }
            public EmployeeDutySchedule ShiftSchedule { get; set; }

            public override string ToString()
            {
                return $"({LogDate.ToShortDateString()}) {ShiftTimeInBounds.ToString("yyyy-MM-dd hh:mm tt")} - {ShiftTimeOutBounds.ToString("yyyy-MM-dd hh:mm tt")}";
            }
        }
    }
}
