
namespace AccuPay.Web.Appraisers
{
    public class ApproverDto
    {
        public int Id { get; set; }

        public int? OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string CompanyName { get; set; }
    }
}
