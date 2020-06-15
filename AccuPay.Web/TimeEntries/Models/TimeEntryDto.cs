using AccuPay.Data.Services;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Web.TimeEntries.Models
{
    public class TimeEntryDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public DatePeriodDto Shift { get; set; }

        public DatePeriodDto TimeLog { get; set; }

        public DatePeriodDto OfficialBusiness { get; set; }

        public decimal RegularHours { get; set; }

        public DatePeriodDto Leave { get; set; }

        public decimal LeaveHours { get; set; }

        public decimal LeavePay { get; set; }

        public List<DatePeriodDto> Overtimes { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimePay { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffPay { get; set; }

        public decimal NightDiffOTHours { get; set; }

        public decimal NightDiffOTPay { get; set; }

        public decimal LateHours { get; set; }

        public decimal LateDeduction { get; set; }

        public decimal RestDayHours { get; set; }

        public decimal RestDayAmount { get; set; }

        public decimal RestDayOTHours { get; set; }

        public decimal RestDayOTPay { get; set; }

        public decimal SpecialHolidayHours { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayHours { get; set; }

        public decimal RegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal UndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal AbsentDeduction { get; set; }

        internal static TimeEntryDto Convert(TimeEntryData data)
        {
            var leaveIsWholeDay = data.Leave != null &&
                                data.Leave.StartTimeFull == null && data.Leave?.EndTimeFull == null;

            return new TimeEntryDto()
            {
                Id = data.Id,
                EmployeeId = data.EmployeeId,
                Date = data.Date,
                Shift = DatePeriodDto.Create(data.Shift?.StartTimeFull, data.Shift?.EndTimeFull),
                TimeLog = DatePeriodDto.Create(data.TimeLog?.TimeInFull, data.TimeLog?.TimeOutFull),
                OfficialBusiness = DatePeriodDto.Create(data.OfficialBusiness?.StartTimeFull, data.OfficialBusiness?.EndTimeFull),
                RegularHours = data.TimeEntry.RegularHours,
                Leave = DatePeriodDto.Create(data.Leave?.StartTimeFull, data.Leave?.EndTimeFull, leaveIsWholeDay),
                LeaveHours = data.TimeEntry.TotalLeaveHours,
                LeavePay = data.TimeEntry.LeavePay,
                Overtimes = data.Overtimes.Select(x => DatePeriodDto.Create(x.OTStartTimeFull, x.OTEndTimeFull)).ToList(),
                OvertimeHours = data.TimeEntry.OvertimeHours,
                OvertimePay = data.TimeEntry.OvertimePay,
                NightDiffHours = data.TimeEntry.NightDiffHours,
                NightDiffPay = data.TimeEntry.NightDiffPay,
                NightDiffOTHours = data.TimeEntry.NightDiffOTHours,
                NightDiffOTPay = data.TimeEntry.NightDiffOTPay,
                RestDayHours = data.TimeEntry.RestDayHours,
                RestDayAmount = data.TimeEntry.RestDayPay,
                RestDayOTHours = data.TimeEntry.RestDayOTHours,
                RestDayOTPay = data.TimeEntry.RestDayOTPay,
                SpecialHolidayHours = data.TimeEntry.SpecialHolidayHours,
                SpecialHolidayPay = data.TimeEntry.SpecialHolidayPay,
                SpecialHolidayOTHours = data.TimeEntry.SpecialHolidayOTHours,
                SpecialHolidayOTPay = data.TimeEntry.SpecialHolidayOTPay,
                RegularHolidayHours = data.TimeEntry.RegularHolidayHours,
                RegularHolidayPay = data.TimeEntry.RegularHolidayPay,
                RegularHolidayOTHours = data.TimeEntry.RegularHolidayOTHours,
                RegularHolidayOTPay = data.TimeEntry.RegularHolidayOTPay,
                LateHours = data.TimeEntry.LateHours,
                LateDeduction = data.TimeEntry.LateDeduction,
                UndertimeHours = data.TimeEntry.UndertimeHours,
                UndertimeDeduction = data.TimeEntry.UndertimeDeduction,
                AbsentHours = data.TimeEntry.AbsentHours,
                AbsentDeduction = data.TimeEntry.AbsentDeduction,
            };
        }
    }
}
