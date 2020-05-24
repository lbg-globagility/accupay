using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Overtimes
{
    public class CreateOvertimeDto : CrudOvertimeDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
