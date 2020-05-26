using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Branches
{
    public class UpdateBranchDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
