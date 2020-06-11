using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.EmploymentPolicies.Models
{
    public class CreateEmploymentPolicyDto
    {
        [Required]
        public string Name { get; set; }
    }
}
