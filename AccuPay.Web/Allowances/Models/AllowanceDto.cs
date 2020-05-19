using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Allowances.Models
{
    class AllowanceDto
    {
        public static AllowanceDto Produce(Allowance allowance)
        {
            return FromAllowancetoDto(allowance);
        }

        internal static AllowanceDto FromAllowancetoDto(Allowance _allowance)
        {
            return new AllowanceDto()
            {
                RowID = _allowance.RowID,
                EmployeeID = _allowance.EmployeeID,
                ProductID = _allowance.ProductID,
                EffectiveStartDate = _allowance.EffectiveStartDate,
                AllowanceFrequency = _allowance.AllowanceFrequency,
                EffectiveEndDate = _allowance.EffectiveEndDate,
                TaxableFlag = _allowance.TaxableFlag,
                Amount = _allowance.Amount
            };
        }

        public int? RowID { get; set; }

        public int? EmployeeID { get; set; }

        public int? ProductID { get; set; }

        public DateTime EffectiveStartDate { get; set; }

        public string AllowanceFrequency { get; set; }

        public DateTime? EffectiveEndDate { get; set; }

        public char TaxableFlag { get; set; }

        public decimal Amount { get; set; }
    }
}
