using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class TotalTimeEntryCalculator
    {
        public static TotalTimeEntryCalculator Calculate(IEnumerable<TimeEntry> timeEntries,
                                                        Salary salary,
                                                        Employee employee,
                                                        IEnumerable<ActualTimeEntry> actualtimeentries)
        {
            return new TotalTimeEntryCalculator(timeEntries, salary, employee, actualtimeentries);
        }

        private TotalTimeEntryCalculator(IEnumerable<TimeEntry> timeEntries,
                                        Salary salary,
                                        Employee employee,
                                        IEnumerable<ActualTimeEntry> actualtimeentries)
        {
            HourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee);
            ActualHourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee, isActual: true);

            RegularHours = timeEntries.Sum(t => t.RegularHours);
            RegularPay = HourlyRate * RegularHours;
            ActualRegularPay = ActualHourlyRate * RegularHours;

            OvertimeHours = timeEntries.Sum(t => t.OvertimeHours);
            OvertimePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.OvertimePay));
            ActualOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.OvertimePay));

            NightDiffHours = timeEntries.Sum(t => t.NightDiffHours);
            NightDiffPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.NightDiffPay));
            ActualNightDiffPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.NightDiffPay));

            NightDiffOvertimeHours = timeEntries.Sum(t => t.NightDiffOTHours);
            NightDiffOvertimePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.NightDiffOTPay));
            ActualNightDiffOvertimePay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.NightDiffOTPay));

            RestDayHours = timeEntries.Sum(t => t.RestDayHours);
            RestDayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RestDayPay));
            ActualRestDayPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.RestDayPay));

            RestDayOTHours = timeEntries.Sum(t => t.RestDayOTHours);
            RestDayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RestDayOTPay));
            ActualRestDayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.RestDayOTPay));

            SpecialHolidayHours = timeEntries.Sum(t => t.SpecialHolidayHours);
            SpecialHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.SpecialHolidayPay));
            ActualSpecialHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.SpecialHolidayPay));

            SpecialHolidayOTHours = timeEntries.Sum(t => t.SpecialHolidayOTHours);
            SpecialHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.SpecialHolidayOTPay));
            ActualSpecialHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.SpecialHolidayOTPay));

            RegularHolidayHours = timeEntries.Sum(t => t.RegularHolidayHours);
            RegularHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RegularHolidayPay));
            ActualRegularHolidayPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.RegularHolidayPay));

            RegularHolidayOTHours = timeEntries.Sum(t => t.RegularHolidayOTHours);
            RegularHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RegularHolidayOTPay));
            ActualRegularHolidayOTPay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.RegularHolidayOTPay));

            HolidayPay = timeEntries.Sum(t => t.HolidayPay);

            LeaveHours = timeEntries.Sum(t => t.TotalLeaveHours);
            LeavePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.LeavePay));
            ActualLeavePay = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.LeavePay));

            LateHours = timeEntries.Sum(t => t.LateHours);
            LateDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.LateDeduction));
            ActualLateDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.LateDeduction));

            UndertimeHours = timeEntries.Sum(t => t.UndertimeHours);
            UndertimeDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.UndertimeDeduction));
            ActualUndertimeDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.UndertimeDeduction));

            AbsentHours = timeEntries.Sum(t => t.AbsentHours);
            AbsenceDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.AbsentDeduction));
            ActualAbsenceDeduction = AccuMath.CommercialRound(actualtimeentries.Sum(t => t.AbsentDeduction));
        }

        public decimal HourlyRate { get; }
        public decimal ActualHourlyRate { get; }
        public decimal RegularHours { get; }
        public decimal RegularPay { get; }
        public decimal ActualRegularPay { get; }
        public decimal OvertimeHours { get; }
        public decimal OvertimePay { get; }
        public decimal ActualOvertimePay { get; }
        public decimal NightDiffHours { get; }
        public decimal NightDiffPay { get; }
        public decimal ActualNightDiffPay { get; }
        public decimal NightDiffOvertimeHours { get; }
        public decimal NightDiffOvertimePay { get; }
        public decimal ActualNightDiffOvertimePay { get; }
        public decimal RestDayHours { get; }
        public decimal RestDayPay { get; }
        public decimal ActualRestDayPay { get; }
        public decimal RestDayOTHours { get; }
        public decimal RestDayOTPay { get; }
        public decimal ActualRestDayOTPay { get; }
        public decimal SpecialHolidayHours { get; }
        public decimal SpecialHolidayPay { get; }
        public decimal ActualSpecialHolidayPay { get; }
        public decimal SpecialHolidayOTHours { get; }
        public decimal SpecialHolidayOTPay { get; }
        public decimal ActualSpecialHolidayOTPay { get; }
        public decimal RegularHolidayHours { get; }
        public decimal RegularHolidayPay { get; }
        public decimal ActualRegularHolidayPay { get; }
        public decimal RegularHolidayOTHours { get; }
        public decimal RegularHolidayOTPay { get; }
        public decimal ActualRegularHolidayOTPay { get; }
        public decimal HolidayPay { get; }
        public decimal LeaveHours { get; }
        public decimal LeavePay { get; }
        public decimal ActualLeavePay { get; }
        public decimal LateHours { get; }
        public decimal LateDeduction { get; }
        public decimal ActualLateDeduction { get; }
        public decimal UndertimeHours { get; }
        public decimal UndertimeDeduction { get; }
        public decimal ActualUndertimeDeduction { get; }
        public decimal AbsentHours { get; }
        public decimal AbsenceDeduction { get; }
        public decimal ActualAbsenceDeduction { get; }
    }
}