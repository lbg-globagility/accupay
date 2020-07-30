using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Allowances.Models
{
    public abstract class CrudAllowanceDto
    {
        private const double MinimumAmount = -99999999.99;
        private const double MaximumAmount = 99999999.99;

        [Required]
        public int AllowanceTypeId { get; set; }

        [Required]
        public string Frequency { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        [Range(MinimumAmount, MaximumAmount)]
        public decimal Amount { get; set; }
    }
}
