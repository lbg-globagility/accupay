using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("allowanceitem")]
    public class AllowanceItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        public virtual int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual int? AllowanceID { get; set; }

        public virtual int? PayPeriodID { get; set; }

        [ForeignKey("Paystub")]
        public virtual int? PaystubID { get; set; }

        public virtual decimal Amount { get; set; }

        [ForeignKey("AllowanceID")]
        public virtual Allowance Allowance { get; set; }

        public virtual Paystub Paystub { get; set; }

        [ForeignKey("PayPeriodID")]
        public virtual PayPeriod PayPeriod { get; set; }

        public virtual IList<AllowancePerDay> AllowancesPerDay { get; set; }

        [NotMapped]
        public bool IsTaxable { get; set; }

        [NotMapped]
        public bool IsThirteenthMonthPay { get; set; }

        public AllowanceItem()
        {
            AllowancesPerDay = new List<AllowancePerDay>();
        }

        public virtual void AddPerDay(DateTime date, decimal amount)
        {
            var perDay = new AllowancePerDay(date, amount);
            perDay.AllowanceItem = this;
            AllowancesPerDay.Add(perDay);

            this.Amount += perDay.Amount;
        }

        public static AllowanceItem Create(
                                Paystub paystub,
                                Product product,
                                int payperiodId,
                                int allowanceId,
                                int organizationId,
                                int userId)
        {
            return new AllowanceItem()
            {
                OrganizationID = organizationId,
                CreatedBy = userId,
                LastUpdBy = userId,
                Paystub = paystub,
                PayPeriodID = payperiodId,
                AllowanceID = allowanceId,
                IsTaxable = product.IsTaxable,
                IsThirteenthMonthPay = product.IsThirteenthMonthPay
            };
        }
    }
}