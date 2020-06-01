using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Account
{
    public class ChangeOrganizationDto
    {
        [Required]
        public int OrganizationId { get; set; }
    }
}
