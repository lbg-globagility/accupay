using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
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