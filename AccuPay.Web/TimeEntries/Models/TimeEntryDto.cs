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

        public List<DatePeriodDto> Overtimes { get; set; }

        public decimal OvertimeHours { get; set; }

        public decimal OvertimePay { get; set; }

        public decimal NightDiffHours { get; set; }

        public decimal NightDiffPay { get; set; }

        public decimal NightDiffOTHours { get; set; }

        public decimal NightDiffOTPay { get; set; }

        public decimal LateHours { get; set; }

        public decimal LateDeduction { get; set; }

        public decimal UndertimeHours { get; set; }

        public decimal UndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }

        public decimal AbsentDeduction { get; set; }

        internal static TimeEntryDto Convert(TimeEntryData data)
        {
            return new TimeEntryDto()
            {
                Id = data.Id,
                EmployeeId = data.EmployeeId,
                Date = data.Date,
                Shift = new DatePeriodDto(data.Shift?.StartTimeFull, data.Shift?.EndTimeFull),
                TimeLog = new DatePeriodDto(data.TimeLog?.TimeInFull, data.TimeLog?.TimeOutFull),
                OfficialBusiness = new DatePeriodDto(data.OfficialBusiness?.StartTimeFull, data.OfficialBusiness?.EndTimeFull),
                RegularHours = data.TimeEntry.RegularHours,
                Overtimes = data.Overtimes.Select(x => new DatePeriodDto(x.OTStartTimeFull, x.OTEndTimeFull)).ToList(),
                OvertimeHours = data.TimeEntry.OvertimeHours,
                OvertimePay = data.TimeEntry.OvertimePay,
                NightDiffHours = data.TimeEntry.NightDiffHours,
                NightDiffPay = data.TimeEntry.NightDiffPay,
                NightDiffOTHours = data.TimeEntry.NightDiffOTHours,
                NightDiffOTPay = data.TimeEntry.NightDiffOTPay,
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
