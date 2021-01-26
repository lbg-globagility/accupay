using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employeebonus")]
    public class Bonus : EmployeeDataEntity
    {
        public const string FREQUENCY_ONE_TIME = "One time";

        public const string FREQUENCY_DAILY = "Daily";

        public const string FREQUENCY_SEMI_MONTHLY = "Semi-monthly";

        public const string FREQUENCY_MONTHLY = "Monthly";
        public int? ProductID { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [Column("EffectiveStartDate")]
        public DateTime EffectiveStartDate { get; set; }

        [Column("EffectiveEndDate")]
        public DateTime EffectiveEndDate { get; set; }

        [Column("AllowanceFrequency")]
        public string AllowanceFrequency { get; set; }

        [Column("TaxableFlag")]
        public string TaxableFlag { get; set; }

        [Column("BonusAmount")]
        public decimal? BonusAmount { get; set; }

        [Column("RemainingBalance")]
        public decimal? RemainingBalance { get; set; }

        public string BonusType
        {
            get
            {
                return Product?.Name;
            }
        }

        public virtual ICollection<LoanPaymentFromBonus> LoanPaymentFromBonuses { get; set; }
    }
}
