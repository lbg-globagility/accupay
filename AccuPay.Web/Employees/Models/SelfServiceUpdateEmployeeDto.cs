using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Employees.Models
{
    public class SelfServiceUpdateEmployeeDto : IValidatableObject
    {
        [Required]
        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public DateTime? Birthdate { get; set; }

        public string Address { get; set; }

        public string LandlineNo { get; set; }

        public string MobileNo { get; set; }

        public string EmailAddress { get; set; }

        public string Tin { get; set; }

        public string SssNo { get; set; }

        public string PhilHealthNo { get; set; }

        public string PagIbigNo { get; set; }

        public string EmploymentStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? RegularizationDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Employee.IsInactiveStatus(EmploymentStatus) && TerminationDate == null)
            {
                yield return new ValidationResult(
                    "terminationDate is required when employmentStatus is Resigned, Terminated or Retired.",
                    new[] { nameof(TerminationDate) });
            }
        }
    }
}
