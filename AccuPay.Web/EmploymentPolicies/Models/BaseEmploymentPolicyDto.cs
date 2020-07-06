using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.EmploymentPolicies.Models
{
    public abstract class BaseEmploymentPolicyDto
    {
        [Required]
        public string Name { get; set; }

        public decimal WorkDaysPerYear { get; set; }

        public decimal GracePeriod { get; set; }

        public bool ComputeNightDiff { get; set; }

        public bool ComputeNightDiffOT { get; set; }

        public bool ComputeRestDay { get; set; }

        public bool ComputeRestDayOT { get; set; }

        public bool ComputeSpecialHoliday { get; set; }

        public bool ComputeRegularHoliday { get; set; }
    }
}
