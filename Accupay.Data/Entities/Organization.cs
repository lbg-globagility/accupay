using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("organization")]
    public class Organization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string Name { get; set; }

        public TimeSpan NightDifferentialTimeFrom { get; set; }

        public TimeSpan NightDifferentialTimeTo { get; set; }

        public bool IsAgency { get; set; }

        public int? PrimaryAddressId { get; set; }

        public int ClientId { get; set; }

        [Column("NoPurpose")]
        public bool IsInActive { get; set; }

        public Byte[] Image { get; set; }

        public static Organization NewOrganization(int userId, int clientId)
        {
            return new Organization()
            {
                CreatedBy = userId,
                ClientId = clientId,
                NightDifferentialTimeFrom = new TimeSpan(22, 0, 0),
                NightDifferentialTimeTo = new TimeSpan(6, 0, 0),
            };
        }
    }
}