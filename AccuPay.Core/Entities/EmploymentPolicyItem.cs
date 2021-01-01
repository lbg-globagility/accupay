using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("employmentpolicyitem")]
    public class EmploymentPolicyItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmploymentPolicyId { get; set; }

        public int EmploymentPolicyTypeId { get; set; }

        public EmploymentPolicyType Type { get; set; }

        public string Value { get; set; }

        public bool IsNew => Id <= 0;

        private EmploymentPolicyItem()
        {
        }

        public EmploymentPolicyItem(EmploymentPolicyType type)
        {
            Type = type;
        }
    }
}
