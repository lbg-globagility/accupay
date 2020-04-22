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
        public virtual int? RowID { get; set; }

        public virtual int? SupplierID { get; set; }

        public virtual string Name { get; set; }

        public virtual int? OrganizationID { get; set; }

        public virtual string Description { get; set; }

        public virtual string PartNo { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual DateTime Created { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public virtual DateTime? LastUpd { get; set; }

        [NotMapped]
        public virtual int? LastArrivedQty { get; set; }

        public virtual int? CreatedBy { get; set; }

        public virtual int? LastUpdBy { get; set; }

        public virtual string Category { get; set; }

        [ForeignKey("CategoryID")]
        public virtual Category CategoryEntity { get; set; }

        public virtual int? CategoryID { get; set; }

        [NotMapped]
        public virtual int? AccountingAccountID { get; set; }

        [NotMapped]
        public virtual string Catalog { get; set; }

        public virtual string Comments { get; set; }

        public virtual string Status { get; set; }

        public virtual bool Fixed { get; set; }

        [NotMapped]
        public virtual decimal UnitPrice { get; set; }

        [NotMapped]
        public virtual decimal VATPercent { get; set; }

        [NotMapped]
        public virtual char FirstBillFlag { get; set; }

        [NotMapped]
        public virtual char SecondBillFlag { get; set; }

        [NotMapped]
        public virtual char ThirdBillFlag { get; set; }

        [NotMapped]
        public virtual char PDCFlag { get; set; }

        [NotMapped]
        public virtual char MonthlyBIllFlag { get; set; }

        [NotMapped]
        public virtual char PenaltyFlag { get; set; }

        [NotMapped]
        public virtual decimal WithholdingTaxPercent { get; set; }

        [NotMapped]
        public virtual decimal CostPrice { get; set; }

        [NotMapped]
        public virtual string UnitOfMeasure { get; set; }

        [NotMapped]
        public virtual string SKU { get; set; }

        [NotMapped]
        public virtual int? LeadTime { get; set; }

        [NotMapped]
        public virtual string BarCode { get; set; }

        [NotMapped]
        public virtual int? BusinessUnitID { get; set; }

        [NotMapped]
        public virtual DateTime LastRcvdFromShipmentDate { get; set; }

        [NotMapped]
        public virtual int? LastRcvdFromShipmentCount { get; set; }

        [NotMapped]
        public virtual int? TotalShipmentCount { get; set; }

        [NotMapped]
        public virtual string BookPageNo { get; set; }

        [NotMapped]
        public virtual string BrandName { get; set; }

        [NotMapped]
        public virtual DateTime LastPurchaseDate { get; set; }

        [NotMapped]
        public virtual DateTime LastSoldDate { get; set; }

        [NotMapped]
        public virtual int? LastSoldCount { get; set; }

        [NotMapped]
        public virtual int? ReOrderPoint { get; set; }

        public virtual char AllocateBelowSafetyFlag { get; set; }

        [NotMapped]
        public virtual string Strength { get; set; }

        [NotMapped]
        public virtual int? UnitsBackordered { get; set; }

        [NotMapped]
        public virtual DateTime UnitsBackorderAsOf { get; set; }

        [NotMapped]
        public virtual DateTime DateLastInventoryCount { get; set; }

        [NotMapped]
        public virtual decimal TaxVAT { get; set; }

        [NotMapped]
        public virtual decimal WithholdingTax { get; set; }

        [NotMapped]
        public virtual int? COAId { get; set; }

        // <NotMapped>
        public virtual bool ActiveData { get; set; }

        public virtual bool IsTaxable
        {
            get
            {
                return Status == "1";
            }
        }

        public bool IsVacationOrSickLeave
        {
            get
            {
                return IsVacationLeave || IsSickLeave;
            }
        }

        public bool IsVacationLeave
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.VACATION_LEAVE.ToUpper();
            }
        }

        public bool IsSickLeave
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.SICK_LEAVE.ToUpper();
            }
        }

        public bool IsMaternityLeave
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.MATERNITY_LEAVE.ToUpper();
            }
        }

        public bool IsParentalLeave
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.PARENTAL_LEAVE.ToUpper();
            }
        }

        public bool IsOthersLeave
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.OTHERS_LEAVE.ToUpper();
            }
        }

        public bool IsPagibigLoan
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.PAG_IBIG_LOAN.ToUpper();
            }
        }

        public bool IsSssLoan
        {
            get
            {
                return PartNo.Trim().ToUpper() == ProductConstant.SSS_LOAN.ToUpper();
            }
        }

        public bool IsThirteenthMonthPay
        {
            get
            {
                return AllocateBelowSafetyFlag == '1';
            }
        }
    }
}