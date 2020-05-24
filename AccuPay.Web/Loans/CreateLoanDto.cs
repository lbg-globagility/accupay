using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Loans
{
    public class CreateLoanDto : CrudLoanDto
    {
        [Required]
        public int EmployeeId { get; set; }
    }
}
