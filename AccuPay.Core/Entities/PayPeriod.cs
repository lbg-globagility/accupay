using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("payperiod")]
    public class PayPeriod : AuditableEntity, IPayPeriod
    {
        public const int FirstPayrollMonth = 1;
        public const int LastPayrollMonth = 12;

        public const int FirstHalfValue = 1;
        public const int EndOftheMonthValue = 0;

        public int? OrganizationID { get; set; }

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

        public bool IsFirstMonthOfTheYear => Month == FirstPayrollMonth;
        public bool IsLastMonthOfTheYear => Month == LastPayrollMonth;
        public bool IsFirstPayPeriodOfTheYear => IsFirstMonthOfTheYear && IsFirstHalf;
        public bool IsLastPayPeriodOfTheYear => IsLastMonthOfTheYear && IsEndOfTheMonth;
        public bool IsOpen => Status == PayPeriodStatus.Open;
        public bool IsClosed => Status == PayPeriodStatus.Closed;
        public bool IsPending => Status == PayPeriodStatus.Pending;

        /// <summary>
        /// The actual first day of the month of the pay period when the salary will be given.
        /// Not the attendance that the salary will be based from.
        /// </summary>
        public DateTime DateMonth => new DateTime(Year, Month, 1);

        public static PayPeriod NewPayPeriod(
            int organizationId,
            int payrollMonth,
            int payrollYear,
            bool isFirstHalf,
            IPolicyHelper policy,
            int? currentlyLoggedInUserId)
        {
            var ordinalValue = payrollMonth * 2;

            var firstHalfDaySpan = policy.DefaultFirstHalfDaysSpan(organizationId);
            var endOfTheMonthDaySpan = policy.DefaultEndOfTheMonthDaysSpan(organizationId);

            DateTime payFromDate, payToDate;

            if (isFirstHalf)
            {
                ordinalValue--;
                payFromDate = firstHalfDaySpan.From.GetDate(month: payrollMonth, year: payrollYear);
                payToDate = firstHalfDaySpan.To.GetDate(month: payrollMonth, year: payrollYear);
            }
            else
            {
                payFromDate = endOfTheMonthDaySpan.From.GetDate(month: payrollMonth, year: payrollYear);
                payToDate = endOfTheMonthDaySpan.To.GetDate(month: payrollMonth, year: payrollYear);
            }

            return new PayPeriod()
            {
                OrganizationID = organizationId,
                PayFrequencyID = PayrollTools.PayFrequencySemiMonthlyId,
                Month = payrollMonth,
                Year = payrollYear,
                Half = isFirstHalf ? FirstHalfValue : EndOftheMonthValue,
                Status = PayPeriodStatus.Pending,
                OrdinalValue = ordinalValue,
                PayFromDate = payFromDate,
                PayToDate = payToDate,
                CreatedBy = currentlyLoggedInUserId
            };
        }
    }
}
