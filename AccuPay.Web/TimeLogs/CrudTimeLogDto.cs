using System;
using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.TimeLogs
{
    public abstract class CrudTimeLogDto
    {
        [Required]
        public DateTime? StartTime { get; set; }

        [Required]
        public DateTime? EndTime { get; set; }

        [Required]
        public int? BranchId { get; set; }
    }
}
