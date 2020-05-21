using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Organizations
{
    public class CreateOrganizationDto
    {
        [Required]
        public string Name { get; set; }

        public string TradeName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
