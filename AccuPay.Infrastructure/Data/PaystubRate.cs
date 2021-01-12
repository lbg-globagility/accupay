using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using System.Collections.Generic;

namespace AccuPay.Infrastructure.Data
{
    public class PaystubRate : IPaystubRate
    {
        public decimal RegularHours { get; set; }
        public decimal RegularPay { get; set; }
        public decimal ActualRegularPay { get; set; }

        public decimal OvertimeHours { get; set; }
        public decimal OvertimePay { get; set; }
        public decimal ActualOvertimePay { get; set; }

        public decimal NightDiffHours { get; set; }
        public decimal NightDiffPay { get; set; }
        public decimal ActualNightDiffPay { get; set; }

        public decimal NightDiffOvertimeHours { get; set; }
        public decimal NightDiffOvertimePay { get; set; }
        public decimal ActualNightDiffOvertimePay { get; set; }

        public decimal RestDayHours { get; set; }
        public decimal RestDayPay { get; set; }
        public decimal ActualRestDayPay { get; set; }

        public decimal RestDayOTHours { get; set; }
        public decimal RestDayOTPay { get; set; }
        public decimal ActualRestDayOTPay { get; set; }

        public decimal SpecialHolidayHours { get; set; }
        public decimal SpecialHolidayPay { get; set; }
        public decimal ActualSpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTHours { get; set; }
        public decimal SpecialHolidayOTPay { get; set; }
        public decimal ActualSpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayHours { get; set; }
        public decimal RegularHolidayPay { get; set; }
        public decimal ActualRegularHolidayPay { get; set; }

        public decimal RegularHolidayOTHours { get; set; }
        public decimal RegularHolidayOTPay { get; set; }
        public decimal ActualRegularHolidayOTPay { get; set; }

        public decimal HolidayPay { get; set; }

        public decimal LeaveHours { get; set; }
        public decimal LeavePay { get; set; }
        public decimal ActualLeavePay { get; set; }

        public decimal LateHours { get; set; }
        public decimal LateDeduction { get; set; }
        public decimal ActualLateDeduction { get; set; }

        public decimal UndertimeHours { get; set; }
        public decimal UndertimeDeduction { get; set; }
        public decimal ActualUndertimeDeduction { get; set; }

        public decimal AbsentHours { get; set; }
        public decimal AbsenceDeduction { get; set; }
        public decimal ActualAbsenceDeduction { get; set; }

        public void Compute(
            IReadOnlyCollection<TimeEntry> timeEntries,
            Salary salary,
            Employee employee,
            IReadOnlyCollection<ActualTimeEntry> actualtimeentries)
        {
            var totalTimeEntries = TotalTimeEntryCalculator.Calculate(timeEntries, salary, employee, actualtimeentries);

            this.RegularHours = totalTimeEntries.RegularHours;
            this.RegularPay = totalTimeEntries.RegularPay;
            this.ActualRegularPay = totalTimeEntries.ActualRegularPay;

            this.OvertimeHours = totalTimeEntries.OvertimeHours;
            this.OvertimePay = totalTimeEntries.OvertimePay;
            this.ActualOvertimePay = totalTimeEntries.ActualOvertimePay;

            this.NightDiffHours = totalTimeEntries.NightDifferentialHours;
            this.NightDiffPay = totalTimeEntries.NightDiffPay;
            this.ActualNightDiffPay = totalTimeEntries.ActualNightDiffPay;

            this.NightDiffOvertimeHours = totalTimeEntries.NightDifferentialOvertimeHours;
            this.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay;
            this.ActualNightDiffOvertimePay = totalTimeEntries.ActualNightDiffOvertimePay;

            this.RestDayHours = totalTimeEntries.RestDayHours;
            this.RestDayPay = totalTimeEntries.RestDayPay;
            this.ActualRestDayPay = totalTimeEntries.ActualRestDayPay;

            this.RestDayOTHours = totalTimeEntries.RestDayOTHours;
            this.RestDayOTPay = totalTimeEntries.RestDayOTPay;
            this.ActualRestDayOTPay = totalTimeEntries.ActualRestDayOTPay;

            this.SpecialHolidayHours = totalTimeEntries.SpecialHolidayHours;
            this.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay;
            this.ActualSpecialHolidayPay = totalTimeEntries.ActualSpecialHolidayPay;

            this.SpecialHolidayOTHours = totalTimeEntries.SpecialHolidayOTHours;
            this.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay;
            this.ActualSpecialHolidayOTPay = totalTimeEntries.ActualSpecialHolidayOTPay;

            this.RegularHolidayHours = totalTimeEntries.RegularHolidayHours;
            this.RegularHolidayPay = totalTimeEntries.RegularHolidayPay;
            this.ActualRegularHolidayPay = totalTimeEntries.ActualRegularHolidayPay;

            this.RegularHolidayOTHours = totalTimeEntries.RegularHolidayOTHours;
            this.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay;
            this.ActualRegularHolidayOTPay = totalTimeEntries.ActualRegularHolidayOTPay;

            this.HolidayPay = totalTimeEntries.HolidayPay;

            this.LeaveHours = totalTimeEntries.LeaveHours;
            this.LeavePay = totalTimeEntries.LeavePay;
            this.ActualLeavePay = totalTimeEntries.ActualLeavePay;

            this.LateHours = totalTimeEntries.LateHours;
            this.LateDeduction = totalTimeEntries.LateDeduction;
            this.ActualLateDeduction = totalTimeEntries.ActualLateDeduction;

            this.UndertimeHours = totalTimeEntries.UndertimeHours;
            this.UndertimeDeduction = totalTimeEntries.UndertimeDeduction;
            this.ActualUndertimeDeduction = totalTimeEntries.ActualUndertimeDeduction;

            this.AbsentHours = totalTimeEntries.AbsentHours;
            this.AbsenceDeduction = totalTimeEntries.AbsenceDeduction;
            this.ActualAbsenceDeduction = totalTimeEntries.ActualAbsenceDeduction;
        }
    }
}
