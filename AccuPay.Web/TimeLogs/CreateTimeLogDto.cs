using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public class CreateTimeLogDto : CrudTimeLogDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
