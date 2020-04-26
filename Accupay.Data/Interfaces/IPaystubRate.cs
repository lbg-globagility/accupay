namespace AccuPay.Data
{
    public interface IPaystubRate
    {
        decimal RegularHours { get; set; }
        decimal RegularPay { get; set; }
        decimal ActualRegularPay { get; set; }

        decimal OvertimeHours { get; set; }
        decimal OvertimePay { get; set; }
        decimal ActualOvertimePay { get; set; }

        decimal NightDiffHours { get; set; }
        decimal NightDiffPay { get; set; }
        decimal ActualNightDiffPay { get; set; }

        decimal NightDiffOvertimeHours { get; set; }
        decimal NightDiffOvertimePay { get; set; }
        decimal ActualNightDiffOvertimePay { get; set; }

        decimal RestDayHours { get; set; }
        decimal RestDayPay { get; set; }
        decimal ActualRestDayPay { get; set; }

        decimal RestDayOTHours { get; set; }
        decimal RestDayOTPay { get; set; }
        decimal ActualRestDayOTPay { get; set; }

        decimal SpecialHolidayHours { get; set; }
        decimal SpecialHolidayPay { get; set; }
        decimal ActualSpecialHolidayPay { get; set; }

        decimal SpecialHolidayOTHours { get; set; }
        decimal SpecialHolidayOTPay { get; set; }
        decimal ActualSpecialHolidayOTPay { get; set; }

        decimal RegularHolidayHours { get; set; }
        decimal RegularHolidayPay { get; set; }
        decimal ActualRegularHolidayPay { get; set; }

        decimal RegularHolidayOTHours { get; set; }
        decimal RegularHolidayOTPay { get; set; }
        decimal ActualRegularHolidayOTPay { get; set; }

        decimal HolidayPay { get; set; }

        decimal LeaveHours { get; set; }
        decimal LeavePay { get; set; }
        decimal ActualLeavePay { get; set; }

        decimal LateHours { get; set; }
        decimal LateDeduction { get; set; }
        decimal ActualLateDeduction { get; set; }

        decimal UndertimeHours { get; set; }
        decimal UndertimeDeduction { get; set; }
        decimal ActualUndertimeDeduction { get; set; }

        decimal AbsentHours { get; set; }
        decimal AbsenceDeduction { get; set; }
        decimal ActualAbsenceDeduction { get; set; }
    }
}