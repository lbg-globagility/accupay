using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Branches
{
    public class CreateBranchDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
