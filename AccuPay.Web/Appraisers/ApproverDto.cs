
using System.Collections.Generic;

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
        public ICollection<EmployeeApproversDto> EmployeeApprovers { get; set; }
        public class EmployeeApproversDto
        {
            public EmployeeDto Employee { get; set; }

        }
        public class EmployeeDto
        {
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }

        }
    }
}
