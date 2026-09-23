using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public abstract class CrudEmployeeTimelogFilingDto : IValidatableObject
    {
        [Required]
        public string EmployeeNumber { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? LunchOut { get; set; }

        public DateTime? LunchIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public string Reason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CheckIn == null && LunchOut == null && LunchIn == null && CheckOut == null)
            {
                yield return new ValidationResult(
                    "At least one of checkIn, lunchOut, lunchIn or checkOut is required.",
                    new[] { nameof(CheckIn), nameof(LunchOut), nameof(LunchIn), nameof(CheckOut) });
            }
        }
    }
}
