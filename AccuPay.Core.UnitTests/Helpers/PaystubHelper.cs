using AccuPay.Core.Entities;

namespace AccuPay.Core.UnitTests
{
    public static class PaystubHelper
    {
        public static void SetHourDeductionsValue(Paystub paystub)
        {
            paystub.AbsentHours = 40;
            paystub.LateHours = 10;
            paystub.UndertimeHours = 20;
        }

        public static void SetTotalWorkedHoursWithoutOvertimeAndLeaveValue(decimal workHoursPerCutOff, Paystub paystub)
        {
            paystub.RestDayHours = 1;
            paystub.SpecialHolidayRestDayHours = 2;
            paystub.RegularHolidayRestDayHours = 3;
            paystub.SpecialHolidayHours = 4;
            paystub.RegularHolidayHours = 5;

            paystub.RegularHours =
                workHoursPerCutOff -
                paystub.RestDayHours -
                paystub.SpecialHolidayRestDayHours -
                paystub.RegularHolidayRestDayHours -
                paystub.SpecialHolidayHours -
                paystub.RegularHolidayHours;
        }
    }
}
