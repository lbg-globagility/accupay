using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("organization")]
    public class Organization : BaseEntity
    {
        public static readonly TimeSpan DefaultNightDifferentialTimeFrom = new TimeSpan(22, 0, 0);
        public static readonly TimeSpan DefaultNightDifferentialTimeTo = new TimeSpan(6, 0, 0);

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string Name { get; set; }

        public string TradeName { get; set; }

        public TimeSpan NightDifferentialTimeFrom { get; set; }

        public TimeSpan NightDifferentialTimeTo { get; set; }

        public bool IsAgency { get; set; }

        public int? PrimaryAddressId { get; set; }

        public int ClientId { get; set; }

        [Column("NoPurpose")]
        public bool IsInActive { get; set; }

        public Byte[] Image { get; set; }

        public string MainPhone { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAddress { get; set; }
        public string RDOCode { get; set; }
        public string ZIPCode { get; set; }
        public string URL { get; set; }
        public string AltPhone { get; set; }
        public string Tinno { get; set; }
        public string AltEmailAddress { get; set; }
        public string OrganizationType { get; set; }

        [ForeignKey("PrimaryAddressId")]
        public Address Address { get; set; }

        public static Organization NewOrganization(int userId, int clientId)
        {
            return new Organization()
            {
                Created = DateTime.Now,
                CreatedBy = userId,
                ClientId = clientId,
                IsAgency = false,
                IsInActive = false,
                NightDifferentialTimeFrom = DefaultNightDifferentialTimeFrom,
                NightDifferentialTimeTo = DefaultNightDifferentialTimeTo,
            };
        }
    }
}