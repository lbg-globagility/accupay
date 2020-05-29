using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccuPay.Web.Divisions.Models
{
    public abstract class CrudDivisionDto
    {
        [Required]
        public string Name { get; set; }

        public string DivisionType { get; set; }

        [Required]
        public int? WorkDaysPerYear { get; set; }

        public bool? AutomaticOvertimeFiling { get; set; }

        public string PhilHealthDeductionSchedule { get; set; }

        public string SssDeductionSchedule { get; set; }

        public string PagIBIGDeductionSchedule { get; set; }

        public string WithholdingTaxSchedule { get; set; }

        [Required]
        public int ParentId { get; set; }

        public string ParentName { get; set; }
    }
}
