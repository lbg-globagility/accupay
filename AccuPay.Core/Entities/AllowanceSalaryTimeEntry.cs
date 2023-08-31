using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeetimeentryallowancesalary")]
    public class AllowanceSalaryTimeEntry : BaseTimeEntry
    {
        public bool IsRestDay { get; set; }

        public bool HasShift { get; set; }

        internal void RecomputeTotalDayPay()
        {
            TotalDayPay = RegularPay +
                LeavePay +
                OvertimePay +
                NightDiffPay +
                NightDiffOTPay +
                RestDayPay +
                RestDayOTPay +
                SpecialHolidayPay +
                SpecialHolidayOTPay +
                RegularHolidayPay +
                RegularHolidayOTPay;
        }
    }
}
