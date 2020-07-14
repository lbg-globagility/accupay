using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("payperiod")]
    public class PayPeriod : BaseEntity, IPayPeriod
    {
        private const int IsJanuary = 1;
        private const int IsDecember = 12;

        public const int FirstHalfValue = 1;
        public const int EndOftheMonthValue = 0;

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        [Column("TotalGrossSalary")]
        public int? PayFrequencyID { get; set; }

        public DateTime PayFromDate { get; set; }

        public DateTime PayToDate { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Half { get; set; }

        public int OrdinalValue { get; set; }

        public PayPeriodStatus Status { get; set; }

        public bool SSSWeeklyContribSched { get; set; }
        public bool PhHWeeklyContribSched { get; set; }
        public bool HDMFWeeklyContribSched { get; set; }
        public bool WTaxWeeklyContribSched { get; set; }

        public bool SSSWeeklyAgentContribSched { get; set; }
        public bool PhHWeeklyAgentContribSched { get; set; }
        public bool HDMFWeeklyAgentContribSched { get; set; }
        public bool WTaxWeeklyAgentContribSched { get; set; }
        public bool IsClosed { get; set; }

        public virtual ICollection<Paystub> Paystubs { get; set; }

        public bool IsSemiMonthly => PayFrequencyID == PayrollTools.PayFrequencySemiMonthlyId;
        public bool IsWeekly => PayFrequencyID == PayrollTools.PayFrequencyWeeklyId;
        public bool IsFirstHalf => Half == FirstHalfValue;
        public bool IsEndOfTheMonth => Half == EndOftheMonthValue;

        public bool IsBetween(DateTime date)
        {
            date = date.ToMinimumHourValue();
            return date >= PayFromDate && date <= PayToDate;
        }

        public bool IsFirstPayPeriodOfTheYear => IsFirstHalf && Month == IsJanuary;
        public bool IsLastPayPeriodOfTheYear => IsEndOfTheMonth && Month == IsDecember;

        public static PayPeriod NewPayPeriod(int organizationId, int month, int year, bool isFirstHalf, PolicyHelper policy)
        {
            var ordinalValue = month * 2;

            var firstHalfDaySpan = policy.DefaultFirstHalfDaysSpan();
            var endOfTheMonthDaySpan = policy.DefaultEndOfTheMonthDaysSpan();

            DateTime payFromDate, payToDate;

            if (isFirstHalf)
            {
                ordinalValue--;
                payFromDate = firstHalfDaySpan.From.GetDate(month: month, year: year);
                payToDate = firstHalfDaySpan.To.GetDate(month: month, year: year);
            }
            else
            {
                payFromDate = endOfTheMonthDaySpan.From.GetDate(month: month, year: year);
                payToDate = endOfTheMonthDaySpan.To.GetDate(month: month, year: year);
            }

            return new PayPeriod()
            {
                OrganizationID = organizationId,
                PayFrequencyID = PayrollTools.PayFrequencySemiMonthlyId,
                Month = month,
                Year = year,
                Half = isFirstHalf ? FirstHalfValue : EndOftheMonthValue,
                Status = PayPeriodStatus.Pending,
                IsClosed = false,
                OrdinalValue = ordinalValue,

                PayFromDate = payFromDate,
                PayToDate = payToDate
            };
        }
    }
}