using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Organizations
{
    public class UpdateOrganizationDto
    {
        [Required]
        public string Name { get; set; }
    }
}
