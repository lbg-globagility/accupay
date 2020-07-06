using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("employmentpolicyitem")]
    public class EmploymentPolicyItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmploymentPolicyId { get; set; }

        public int EmploymentPolicyTypeId { get; set; }

        public string Value { get; set; }
    }
}
