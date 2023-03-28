using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("organization")]
    public class Organization : AuditableEntity
    {
        public static readonly TimeSpan DefaultNightDifferentialTimeFrom = new TimeSpan(22, 0, 0);
        public static readonly TimeSpan DefaultNightDifferentialTimeTo = new TimeSpan(6, 0, 0);

        public string Name { get; set; }

        public string TradeName { get; set; }

        public TimeSpan NightDifferentialTimeFrom { get; set; }

        public TimeSpan NightDifferentialTimeTo { get; set; }

        public bool IsAgency { get; set; }

        public int? PrimaryAddressId { get; set; }

        public int ClientId { get; set; }

        [Column("NoPurpose")]
        public bool IsInactive { get; set; }

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

        public bool PaidAsLongAsHasTimeLog { get; set; }

        public ICollection<Category> Categories { get; set; }

        public int PayFrequencyID { get; set; }

        [ForeignKey("PayFrequencyID")]
        public virtual PayFrequency PayFrequency { get; set; }

        public bool IsWeekly => PayFrequency != null && PayFrequency.IsWeekly;

        public bool IsSemiMonthly => PayFrequency != null && PayFrequency.IsSemiMonthly;

        public bool IsAtm => Name.ToLower().Contains("atm");

        public bool IsCash => Name.ToLower().Contains("cash");

        public static Organization NewOrganization(int clientId)
        {
            return new Organization()
            {
                ClientId = clientId,
                IsAgency = false,
                IsInactive = false,
                NightDifferentialTimeFrom = DefaultNightDifferentialTimeFrom,
                NightDifferentialTimeTo = DefaultNightDifferentialTimeTo,
            };
        }
    }
}
