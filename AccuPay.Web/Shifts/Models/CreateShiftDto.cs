using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Shifts.Models
{
    public class CreateShiftDto : CrudShiftDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
