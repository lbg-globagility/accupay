using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("contractor")]
    public partial class Contractor : OrganizationalEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null;

        [StringLength(255)]
        public string Address { get; set; }

        [StringLength(50)]
        public string TIN { get; set; }

        [StringLength(50)]
        public string ContactInfo { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public virtual ICollection<ContractorProject> ContractorProjects { get; set; } = new List<ContractorProject>();
    }

    public partial class Contractor
    {
        private Contractor() { }

        public Contractor(int userId,
            string name,
            string address,
            string tin,
            string contactInfo,
            string description)
        {
            Name = name;
            Address = address;
            TIN = tin;
            ContactInfo = contactInfo;
            Description = description;
            OrganizationID = 0;

            if(IsNewEntity) CreatedBy = userId;

            LastUpdBy = userId;
        }

        public static Contractor Create(int userId,
            string name = "Contractor ABC123",
            string address = "",
            string tin = "",
            string contactInfo = "",
            string description = "")
            => new Contractor(userId: userId,
                name: name,
                address: address,
                tin: tin,
                contactInfo: contactInfo,
                description: description);

        public static Contractor CloneFrom(int userId, Contractor originContractor)
        {
            var clone = Create(userId: userId,
                name: originContractor.Name,
                address: originContractor.Address,
                tin: originContractor.TIN,
                contactInfo: originContractor.ContactInfo,
                description: originContractor.Description);

            clone.RowID = originContractor.RowID;

            return clone;
        }
    }
}
