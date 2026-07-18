using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Appraisers
{
    public class UpdateApproverDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string CompanyName { get; set; }
    }
}