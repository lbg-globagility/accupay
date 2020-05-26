using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Leaves
{
    public class CreateLeaveDto : CrudLeaveDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
