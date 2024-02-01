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

        public static void SetPayDeductionsValue(Paystub paystub)
        {
            paystub.AbsenceDeduction = 400;
            paystub.LateDeduction = 100;
            paystub.UndertimeDeduction = 200;
        }

        public static void SetTotalWorkedPayWithoutOvertimeAndLeaveValue(decimal workPayPerCutOff, Paystub paystub)
        {
            paystub.RestDayPay = 10;
            paystub.SpecialHolidayRestDayPay = 20;
            paystub.RegularHolidayRestDayPay = 30;
            paystub.SpecialHolidayPay = 40;
            paystub.RegularHolidayPay = 50;

            paystub.RegularPay =
                workPayPerCutOff -
                paystub.RestDayPay -
                paystub.SpecialHolidayRestDayPay -
                paystub.RegularHolidayRestDayPay -
                paystub.SpecialHolidayPay -
                paystub.RegularHolidayPay;
        }
    }
}
