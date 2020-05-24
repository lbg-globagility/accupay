using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employeeallowance")]
    public class Allowance
    {
        public const string FREQUENCY_ONE_TIME = "One time";

        public const string FREQUENCY_DAILY = "Daily";

        public const string FREQUENCY_SEMI_MONTHLY = "Semi-monthly";

        public const string FREQUENCY_MONTHLY = "Monthly";

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

        public int? EmployeeID { get; set; }

        public int? ProductID { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        [Column("AllowanceAmount")]
        public decimal Amount { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [NotMapped]
        public string Type
        {
            get => Product?.PartNo;
            set
            {
                if (Product != null)
                    Product.PartNo = value;
            }
        }

        public bool IsOneTime => AllowanceFrequency == FREQUENCY_ONE_TIME;

        public bool IsDaily => AllowanceFrequency == FREQUENCY_DAILY;

        public bool IsSemiMonthly => AllowanceFrequency == FREQUENCY_SEMI_MONTHLY;

        public bool IsMonthly => AllowanceFrequency == FREQUENCY_MONTHLY;
    }
}