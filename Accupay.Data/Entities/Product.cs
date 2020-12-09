using AccuPay.Data.Helpers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("product")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public string Name { get; set; }

        public int? OrganizationID { get; set; }

        public string Description { get; set; }

        public string PartNo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? CreatedBy { get; set; }

        public int? LastUpdBy { get; set; }

        public string Category { get; set; }

        [ForeignKey("CategoryID")]
        public Category CategoryEntity { get; set; }

        public int? CategoryID { get; set; }

        public string Comments { get; set; }

        public string Status { get; set; }

        public bool Fixed { get; set; }

        public char AllocateBelowSafetyFlag { get; set; }

        public bool ActiveData { get; set; }

        public string DisplayName => PartNo;

        public bool IsTaxable => Status == "1";

        public bool IsVacationOrSickLeave => IsVacationLeave || IsSickLeave;

        public bool IsVacationLeave => PartNo.Trim().ToUpper() == ProductConstant.VACATION_LEAVE.ToUpper();

        public bool IsSickLeave => PartNo.Trim().ToUpper() == ProductConstant.SICK_LEAVE.ToUpper();

        public bool IsMaternityLeave => PartNo.Trim().ToUpper() == ProductConstant.MATERNITY_LEAVE.ToUpper();

        public bool IsParentalLeave => PartNo.Trim().ToUpper() == ProductConstant.PARENTAL_LEAVE.ToUpper();

        public bool IsOthersLeave => PartNo.Trim().ToUpper() == ProductConstant.OTHERS_LEAVE.ToUpper();

        public bool IsPagibigLoan => PartNo.Trim().ToUpper() == ProductConstant.PAG_IBIG_LOAN.ToUpper();

        public bool IsSssLoan => PartNo.Trim().ToUpper() == ProductConstant.SSS_LOAN.ToUpper();

        public bool IsThirteenthMonthPay => AllocateBelowSafetyFlag == '1';
    }
}
