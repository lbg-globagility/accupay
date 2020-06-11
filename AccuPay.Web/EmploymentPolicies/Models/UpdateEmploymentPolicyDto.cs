using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.EmploymentPolicies.Models
{
    public class UpdateEmploymentPolicyDto
    {
        [Required]
        public string Name { get; set; }
    }
}
