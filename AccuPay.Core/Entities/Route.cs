using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("route")]
    public class Route
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string Description { get; set; }

        public decimal Distance { get; set; }

        public static Route NewRoute(int organizationId, int userId)
        {
            return new Route() { OrganizationID = organizationId, CreatedBy = userId };
        }
    }
}