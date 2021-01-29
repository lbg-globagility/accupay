using AccuPay.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    public abstract class BasePaystub
    {
        public int? OrganizationID { get; set; }

        public int? PayPeriodID { get; set; }

        public int? EmployeeID { get; set; }

        public DateTime PayFromDate { get; set; }

        public DateTime PayToDate { get; set; }

        public decimal RegularPay { get; set; }

        public decimal LateDeduction { get; set; }

        public decimal UndertimeDeduction { get; set; }

        public decimal AbsenceDeduction { get; set; }

        public decimal OvertimePay { get; set; }

        public decimal NightDiffPay { get; set; }

        public decimal NightDiffOvertimePay { get; set; }

        public decimal RestDayPay { get; set; }

        public decimal RestDayOTPay { get; set; }

        public decimal LeavePay { get; set; }

        public decimal SpecialHolidayPay { get; set; }

        public decimal SpecialHolidayOTPay { get; set; }

        public decimal RegularHolidayPay { get; set; }

        public decimal RegularHolidayOTPay { get; set; }

        public decimal RestDayNightDiffPay { get; set; }
        public decimal RestDayNightDiffOTPay { get; set; }

        public decimal SpecialHolidayNightDiffPay { get; set; }
        public decimal SpecialHolidayNightDiffOTPay { get; set; }
        public decimal SpecialHolidayRestDayPay { get; set; }
        public decimal SpecialHolidayRestDayOTPay { get; set; }
        public decimal SpecialHolidayRestDayNightDiffPay { get; set; }
        public decimal SpecialHolidayRestDayNightDiffOTPay { get; set; }

        public decimal RegularHolidayNightDiffPay { get; set; }
        public decimal RegularHolidayNightDiffOTPay { get; set; }
        public decimal RegularHolidayRestDayPay { get; set; }
        public decimal RegularHolidayRestDayOTPay { get; set; }
        public decimal RegularHolidayRestDayNightDiffPay { get; set; }
        public decimal RegularHolidayRestDayNightDiffOTPay { get; set; }

        [Column("TotalGrossSalary")]
        public decimal GrossPay { get; set; }

        public decimal TotalAdjustments { get; set; }

        [Column("TotalNetSalary")]
        public decimal NetPay { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        public decimal BasicPay { get; internal set; }

        public decimal BasicDeductions => LateDeduction + UndertimeDeduction + AbsenceDeduction;

        public decimal TotalEarningForDaily => RegularPay + LeavePay + AdditionalPay;

        public void ComputeBasicPay(bool isDaily, decimal salary, IReadOnlyCollection<BaseTimeEntry> timeEntries, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
        {
            if (isDaily)
            {
                BasicPay = timeEntries.Sum(t => t.BasicDayPay);
            }
            else
            {
                BasicPay = AccuMath.CommercialRound(value: salary / 2, midpointRounding: midpointRounding);
            }
        }

        public void ComputeBasicPay(decimal basicHours, decimal hourlyRate)
        {
            BasicPay = basicHours * hourlyRate;
        }

        public decimal AdditionalPay
        {
            get
            {
                var original = OvertimePay +
                                NightDiffPay +
                                NightDiffOvertimePay +
                                RestDayPay +
                                RestDayOTPay +
                                SpecialHolidayPay +
                                SpecialHolidayOTPay +
                                RegularHolidayPay +
                                RegularHolidayOTPay;

                var newBreakdowns = RestDayNightDiffPay +
                                    RestDayNightDiffOTPay +
                                    SpecialHolidayNightDiffPay +
                                    SpecialHolidayNightDiffOTPay +
                                    SpecialHolidayRestDayPay +
                                    SpecialHolidayRestDayOTPay +
                                    SpecialHolidayRestDayNightDiffPay +
                                    SpecialHolidayRestDayNightDiffOTPay +
                                    RegularHolidayNightDiffPay +
                                    RegularHolidayNightDiffOTPay +
                                    RegularHolidayRestDayPay +
                                    RegularHolidayRestDayOTPay +
                                    RegularHolidayRestDayNightDiffPay +
                                    RegularHolidayRestDayNightDiffOTPay;

                return original + newBreakdowns;
            }
        }
    }
}
