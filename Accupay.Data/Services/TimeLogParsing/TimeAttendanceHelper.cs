using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class TimeAttendanceHelper : ITimeAttendanceHelper
    {
        private const double HOURS_BUFFER = 4;

        private List<ImportTimeAttendanceLog> _importedTimeAttendanceLogs;
        private List<IGrouping<string, ImportTimeAttendanceLog>> _logsGroupedByEmployee;

        private List<Employee> _employees;
        private List<ShiftSchedule> _employeeShifts;

        private readonly int _organizationId;
        private readonly int _userId;

        public TimeAttendanceHelper(List<ImportTimeAttendanceLog> importedTimeLogs,
                                    List<Employee> employees,
                                    List<ShiftSchedule> employeeShifts,
                                    int organizationId,
                                    int userId)
        {
            _importedTimeAttendanceLogs = importedTimeLogs;
            _employees = employees;
            _employeeShifts = employeeShifts;
            _organizationId = organizationId;
            _userId = userId;
            _logsGroupedByEmployee = ImportTimeAttendanceLog.GroupByEmployee(_importedTimeAttendanceLogs);

            GetEmployeeObjectOfLogs();
        }

        public List<ImportTimeAttendanceLog> Analyze()
        {
            var dayLogRecords = GenerateDayLogRecords();

            foreach (var dayLogRecord in dayLogRecords)
            {
                var timeAttendanceLogs = dayLogRecord.LogRecords.
                                                        Where(d => d.IsTimeIn == null).
                                                        OrderBy(d => d.DateTime).
                                                        ToList();

                var index = 0;
                var lastIndex = timeAttendanceLogs.Count() - 1;

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

                    timeAttendanceLog.ShiftSchedule = dayLogRecord.ShiftSchedule;

                    index += 1;
                }
            }

            return _importedTimeAttendanceLogs;
        }

        public List<ImportTimeAttendanceLog> Validate()
        {
            // NOTHING YET

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
                    var logsByDayList = logsByDay.OrderBy(l => l.DateTime).ToList();

                    if (logsByDay.Key == null)
                        continue;

                    var logDate = Convert.ToDateTime(logsByDay.Key);

                    var firstTimeStampIn = logsByDayList.
                                            FirstOrDefault(l => Nullable.Equals(l.IsTimeIn, true))?.
                                            DateTime;
                    var finalTimeStampOut = logsByDayList.
                                            LastOrDefault(l => Nullable.Equals(l.IsTimeIn, false))?.
                                            DateTime;

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
                        EmployeeID = currentEmployee.RowID,
                        TimeIn = firstTimeIn,
                        TimeOut = finalTimeOut,
                        TimeStampIn = firstTimeStampIn,
                        TimeStampOut = finalTimeStampOut,
                        BranchID = currentEmployee.BranchID
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
                if (logGroup.Count() == 0) continue;

                var employeeId = logGroup.FirstOrDefault().Employee?.RowID;

                if (employeeId == null) continue;

                var currentEmployeeShifts = _employeeShifts.
                                                Where(s => s.EmployeeID == employeeId).
                                                ToList();

                var employeeLogs = logGroup.ToList();

                var earliestDate = logGroup.FirstOrDefault().DateTime.Date;
                var lastDate = logGroup.LastOrDefault().DateTime.Date;

                foreach (var currentDate in CalendarHelper.EachDay(earliestDate, lastDate))
                {
                    var dayLogRecord = GenerateDayLogRecord(employeeLogs, currentEmployeeShifts, currentDate);

                    if (dayLogRecord != null)
                        dayLogRecords.Add(dayLogRecord);
                }
            }

            return dayLogRecords;
        }

        private DayLogRecord GenerateDayLogRecord(List<ImportTimeAttendanceLog> employeeLogs, List<ShiftSchedule> currentEmployeeShifts, DateTime currentDate)
        {
            var currentShift = GetShift(currentEmployeeShifts, currentDate);

            var nextDate = currentDate.AddDays(1);
            var nextShift = GetShift(currentEmployeeShifts, nextDate);

            var timeInBounds = GetShiftBoundsForTimeIn(currentDate, currentShift);
            var timeOutBounds = GetShiftBoundsForTimeOut(currentDate, currentShift, nextShift);

            var dayBounds = new TimePeriod(timeInBounds, timeOutBounds);

            return new DayLogRecord()
            {
                LogDate = currentDate,
                LogRecords = GetTimeLogsFromBounds(dayBounds, employeeLogs),
                ShiftTimeInBounds = dayBounds.Start,
                ShiftTimeOutBounds = dayBounds.End,
                ShiftSchedule = currentShift
            };
        }

        private ShiftSchedule GetShift(List<ShiftSchedule> currentEmployeeShifts, DateTime currentDate)
        {
            return currentEmployeeShifts.
                    Where(s => s.EffectiveFrom <= currentDate && currentDate <= s.EffectiveTo).
                    FirstOrDefault();
        }

        private List<ImportTimeAttendanceLog> GetTimeLogsFromBounds(TimePeriod shiftBounds, List<ImportTimeAttendanceLog> timeAttendanceLogs
    )
        {
            return timeAttendanceLogs.Where(t =>
            {
                return t.DateTime >= shiftBounds.Start && t.DateTime <= shiftBounds.End;
            }).ToList();
        }

        private DateTime GetShiftBoundsForTimeIn(DateTime currentDate, ShiftSchedule currentShift)
        {
            DateTime shiftMinBound;

            DateTime shiftTimeFrom;

            if (currentShift == null || currentShift.Shift == null)
            {
                // If walang shift, minimum bound should be 12:00 AM
                shiftTimeFrom = currentDate.ToMinimumHourValue();
                shiftMinBound = shiftTimeFrom;
            }
            else
            {
                // If merong shift, minimum bound should be shift.TimeFrom - 4 hours
                // (ex. 9:00 AM - 5:00 PM shift -> 5:00 AM minimum bound)
                shiftTimeFrom = currentDate.Add(currentShift.Shift.TimeFrom);
                shiftMinBound = shiftTimeFrom.Add(TimeSpan.FromHours(-HOURS_BUFFER));
            }

            return shiftMinBound;
        }

        private DateTime GetShiftBoundsForTimeOut(DateTime currentDate, ShiftSchedule currentShift, ShiftSchedule nextShift)
        {
            DateTime shiftMaxBound;
            TimeSpan maxBoundTime;

            if (nextShift != null && nextShift.Shift != null)
            {
                // If merong next shift, maximum bound should be
                // (nextShift.TimeFrom - 4 hours - 1 second) + 1 day
                // ex. 9:00 AM - 5:00 PM -> (next day) 4:59 AM maximum bound
                maxBoundTime = nextShift.Shift.TimeFrom.Add(TimeSpan.FromHours(-HOURS_BUFFER)).Add(TimeSpan.FromSeconds(-1));

                shiftMaxBound = currentDate.AddDays(1).Add(maxBoundTime);
            }
            else if (currentShift != null && currentShift.Shift != null)
            {
                var dateTimeOut = currentDate;

                // check if shift is night shift by checking
                // if TimeFrom is greater than TimeTo
                if (currentShift.Shift.TimeFrom >= currentShift.Shift.TimeTo)
                {
                    // if night shift, then dateTimeOut should be in the next day
                    dateTimeOut = dateTimeOut.AddDays(1);

                    // maxBoundTime should be shift TimeTo plus 4 hours
                    // (ex. 9:00 PM - 5:00 AM -> (next day) 9:00 AM
                    maxBoundTime = currentShift.Shift.TimeTo.Add(TimeSpan.FromHours(HOURS_BUFFER));
                }
                else

                    // if not night shift, then dateTimeOut should be same day
                    // and make the maxBoundTime the maximum hours for that day
                    // (ex. 9:00 AM - 5:00 PM -> 11:59 PM maximum bound)
                    maxBoundTime = new TimeSpan(23, 59, 59);

                shiftMaxBound = dateTimeOut.Add(maxBoundTime);
            }
            else

                // If walang next shift and walang current end time shift
                // maximum bound should be 11:59 PM
                shiftMaxBound = currentDate.ToMaximumHourValue();

            return shiftMaxBound;
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

        private class DayLogRecord
        {
            public DateTime LogDate { get; set; }

            public List<ImportTimeAttendanceLog> LogRecords { get; set; }

            public DateTime ShiftTimeInBounds { get; set; }

            public DateTime ShiftTimeOutBounds { get; set; }

            public ShiftSchedule ShiftSchedule { get; set; }

            public override string ToString()
            {
                return $"({LogDate.ToShortDateString()}) {ShiftTimeInBounds.ToString("yyyy-MM-dd hh:mm tt")} - {ShiftTimeOutBounds.ToString("yyyy-MM-dd hh:mm tt")}";
            }
        }
    }
}