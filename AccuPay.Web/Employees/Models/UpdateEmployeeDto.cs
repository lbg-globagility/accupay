using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Employees.Models
{
    public class UpdateEmployeeDto
    {
        [Required]
        public string EmployeeNo { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

        public string LandlineNo { get; set; }

        public string MobileNo { get; set; }

        public string EmailAddress { get; set; }

        public string Tin { get; set; }

        public string SssNo { get; set; }

        public string PhilHealthNo { get; set; }

        public string PagIbigNo { get; set; }

        [Required]
        public string EmploymentStatus { get; set; }

        public string EmploymentPolicy { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? RegularizationDate { get; set; }

        public int? EmploymentPolicyId { get; set; }
    }
}
