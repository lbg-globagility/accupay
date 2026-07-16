using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("approver")]
    public class Approver : BaseEntity
    {
        public int? OrganizationID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string CompanyName { get; set; }

        [ForeignKey("OrganizationID")]
        public virtual Organization Organization { get; set; }
    }
}
