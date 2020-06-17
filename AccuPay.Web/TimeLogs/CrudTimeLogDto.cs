using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public abstract class CrudTimeLogDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? BranchId { get; set; }
    }
}
