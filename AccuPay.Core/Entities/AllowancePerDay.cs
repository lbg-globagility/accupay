using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("allowanceperday")]
    public class AllowancePerDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int? RowID { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual decimal Amount { get; set; }

        [ForeignKey("AllowanceItemID")]
        public virtual AllowanceItem AllowanceItem { get; set; }

        protected AllowancePerDay()
        {
        }

        public AllowancePerDay(DateTime date, decimal amount)
        {
            this.Date = date;
            this.Amount = amount;
        }
    }
}