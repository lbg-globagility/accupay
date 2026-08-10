using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public class SelfServiceCreateTimeLogDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? LunchOut { get; set; }

        public DateTime? LunchIn { get; set; }

        public int? BranchId { get; set; }
    }
}
