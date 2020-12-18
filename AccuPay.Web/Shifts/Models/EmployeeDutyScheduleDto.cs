using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Shifts.Models
{
    public class EmployeeDutyScheduleDto
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakStartTime { get; set; }
        public decimal BreakLength { get; set; }
        public bool IsOffset { get; set; }
        public decimal ShiftHours { get; internal set; }
        public decimal WorkHours { get; internal set; }

        internal static EmployeeDutyScheduleDto Convert(EmployeeDutySchedule dutySchedule)
        {
            return new EmployeeDutyScheduleDto()
            {
                BreakLength = dutySchedule.BreakLength,
                BreakStartTime = dutySchedule.ShiftBreakStartTimeFull,
                Date = dutySchedule.DateSched,
                EmployeeId = dutySchedule.EmployeeID,
                EndTime = dutySchedule.EndTimeFull,
                IsOffset = dutySchedule.IsRestDay,
                Id = dutySchedule.RowID,
                ShiftHours = dutySchedule.ShiftHours,
                StartTime = dutySchedule.StartTimeFull,
                WorkHours = dutySchedule.WorkHours
            };
        }
    }
}
