using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("payperiod")]
    public class PayPeriod : IPayPeriod
    {
        private const int IsJanuary = 1;
        private const int IsDecember = 12;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

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

        public bool SSSWeeklyContribSched { get; set; }
        public bool PhHWeeklyContribSched { get; set; }
        public bool HDMFWeeklyContribSched { get; set; }
        public bool WTaxWeeklyContribSched { get; set; }

        public bool SSSWeeklyAgentContribSched { get; set; }
        public bool PhHWeeklyAgentContribSched { get; set; }
        public bool HDMFWeeklyAgentContribSched { get; set; }
        public bool WTaxWeeklyAgentContribSched { get; set; }
        public bool IsClosed { get; set; }

        public PayPeriod NextPayPeriod()
        {
            // transfer this to a repository, no database call in entity
            if (this.RowID == null) return null;

            using (var context = new PayrollContext())
            {
                return context.PayPeriods.
                                Where(p => p.OrganizationID.Value == this.OrganizationID.Value).
                                Where(p => p.PayFrequencyID.Value == this.PayFrequencyID.Value).
                                Where(p => p.PayFromDate > this.PayFromDate).
                                OrderBy(p => p.PayFromDate).
                                FirstOrDefault();
            }
        }

        public bool IsSemiMonthly => PayFrequencyID.Value == PayrollTools.PayFrequencySemiMonthlyId;
        public bool IsWeekly => PayFrequencyID.Value == PayrollTools.PayFrequencyWeeklyId;
        public bool IsFirstHalf => Half == 1;
        public bool IsEndOfTheMonth => Half == 0;

        public bool IsBetween(DateTime date) => date >= PayFromDate && date <= PayToDate;

        public bool IsFirstPayPeriodOfTheYear => IsFirstHalf && Month == IsJanuary;
        public bool IsLastPayPeriodOfTheYear => IsEndOfTheMonth && Month == IsDecember;
    }
}