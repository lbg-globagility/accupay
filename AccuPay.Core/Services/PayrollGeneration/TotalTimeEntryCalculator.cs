using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.Services
{
    public class TotalTimeEntryCalculator : TotalTimeEntryHours
    {
        public static TotalTimeEntryCalculator Calculate(IEnumerable<TimeEntry> timeEntries,
                                                        Salary salary,
                                                        Employee employee,
                                                        IEnumerable<ActualTimeEntry> actualtimeentries)
        {
            return new TotalTimeEntryCalculator(timeEntries, salary, employee, actualtimeentries);
        }

        public static TotalTimeEntryHours CalculateHours(IEnumerable<TimeEntry> timeEntries)
        {
            Salary salary = null;
            Employee employee = null;
            IEnumerable<ActualTimeEntry> actualtimeentries = null;
            TotalTimeEntryHours totalHours = new TotalTimeEntryCalculator(timeEntries, salary, employee, actualtimeentries);

            return totalHours;
        }

        private TotalTimeEntryCalculator(IEnumerable<TimeEntry> timeEntries,
                                        Salary salary,
                                        Employee employee,
                                        IEnumerable<ActualTimeEntry> actualTimeEntries)
        {
            if (timeEntries == null || !timeEntries.Any())
                return;

            RegularHours = timeEntries.Sum(t => t.RegularHours);
            OvertimeHours = timeEntries.Sum(t => t.OvertimeHours);
            NightDifferentialHours = timeEntries.Sum(t => t.NightDiffHours);
            NightDifferentialOvertimeHours = timeEntries.Sum(t => t.NightDiffOTHours);
            RestDayHours = timeEntries.Sum(t => t.RestDayHours);
            RestDayOTHours = timeEntries.Sum(t => t.RestDayOTHours);
            SpecialHolidayHours = timeEntries.Sum(t => t.SpecialHolidayHours);
            SpecialHolidayOTHours = timeEntries.Sum(t => t.SpecialHolidayOTHours);
            RegularHolidayHours = timeEntries.Sum(t => t.RegularHolidayHours);
            RegularHolidayOTHours = timeEntries.Sum(t => t.RegularHolidayOTHours);
            LeaveHours = timeEntries.Sum(t => t.TotalLeaveHours);
            LateHours = timeEntries.Sum(t => t.LateHours);
            UndertimeHours = timeEntries.Sum(t => t.UndertimeHours);
            AbsentHours = timeEntries.Sum(t => t.AbsentHours);

            if (salary == null || employee == null)
                return;

            HourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee);
            ActualHourlyRate = PayrollTools.GetHourlyRateByDailyRate(salary, employee, isActual: true);

            //RegularPay = HourlyRate * RegularHours;
            RegularPay = timeEntries.Sum(t => t.RegularPay);
            ActualRegularPay = ActualHourlyRate * RegularHours;

            if (timeEntries != null && timeEntries.Any())
            {
                OvertimePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.OvertimePay));
                NightDiffPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.NightDiffPay));
                NightDiffOvertimePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.NightDiffOTPay));
                RestDayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RestDayPay));
                RestDayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RestDayOTPay));
                SpecialHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.SpecialHolidayPay));
                SpecialHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.SpecialHolidayOTPay));
                RegularHolidayPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RegularHolidayPay));
                RegularHolidayOTPay = AccuMath.CommercialRound(timeEntries.Sum(t => t.RegularHolidayOTPay));
                LeavePay = AccuMath.CommercialRound(timeEntries.Sum(t => t.LeavePay));
                LateDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.LateDeduction));
                UndertimeDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.UndertimeDeduction));
                AbsenceDeduction = AccuMath.CommercialRound(timeEntries.Sum(t => t.AbsentDeduction));
            }

            if (actualTimeEntries != null && actualTimeEntries.Any())
            {
                ActualOvertimePay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.OvertimePay));
                ActualNightDiffPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.NightDiffPay));
                ActualNightDiffOvertimePay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.NightDiffOTPay));
                ActualRestDayPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.RestDayPay));
                ActualRestDayOTPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.RestDayOTPay));
                ActualSpecialHolidayPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.SpecialHolidayPay));
                ActualSpecialHolidayOTPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.SpecialHolidayOTPay));
                ActualRegularHolidayPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.RegularHolidayPay));
                ActualRegularHolidayOTPay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.RegularHolidayOTPay));
                ActualLeavePay = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.LeavePay));
                ActualLateDeduction = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.LateDeduction));
                ActualUndertimeDeduction = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.UndertimeDeduction));
                ActualAbsenceDeduction = AccuMath.CommercialRound(actualTimeEntries.Sum(t => t.AbsentDeduction));
            }
        }

        public decimal HourlyRate { get; }
        public decimal ActualHourlyRate { get; }
        public decimal RegularPay { get; }
        public decimal ActualRegularPay { get; }
        public decimal OvertimePay { get; }
        public decimal ActualOvertimePay { get; }
        public decimal NightDiffPay { get; }
        public decimal ActualNightDiffPay { get; }
        public decimal NightDiffOvertimePay { get; }
        public decimal ActualNightDiffOvertimePay { get; }
        public decimal RestDayPay { get; }
        public decimal ActualRestDayPay { get; }
        public decimal RestDayOTPay { get; }
        public decimal ActualRestDayOTPay { get; }
        public decimal SpecialHolidayPay { get; }
        public decimal ActualSpecialHolidayPay { get; }
        public decimal SpecialHolidayOTPay { get; }
        public decimal ActualSpecialHolidayOTPay { get; }
        public decimal RegularHolidayPay { get; }
        public decimal ActualRegularHolidayPay { get; }
        public decimal RegularHolidayOTPay { get; }
        public decimal ActualRegularHolidayOTPay { get; }
        public decimal HolidayPay { get; }
        public decimal LeavePay { get; }
        public decimal ActualLeavePay { get; }
        public decimal LateDeduction { get; }
        public decimal ActualLateDeduction { get; }
        public decimal UndertimeDeduction { get; }
        public decimal ActualUndertimeDeduction { get; }
        public decimal AbsenceDeduction { get; }
        public decimal ActualAbsenceDeduction { get; }
    }
}
