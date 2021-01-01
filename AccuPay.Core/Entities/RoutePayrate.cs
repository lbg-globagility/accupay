using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("routepayrate")]
    public class RoutePayRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; private set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; private set; }

        public int? LastUpdBy { get; set; }

        public int? RouteID { get; set; }

        public int? PositionID { get; set; }

        public decimal Rate { get; set; }

        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        public bool IsNew => (RowID ?? 0) <= 0;
    }
}