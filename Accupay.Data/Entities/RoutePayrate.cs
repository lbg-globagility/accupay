using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("routepayrate")]
    public class RoutePayRate
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? RouteID { get; set; }

        public int? PositionID { get; set; }

        public decimal Rate { get; set; }

        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }

        [ForeignKey("PositionID")]
        public virtual Position Position { get; set; }

        public RoutePayRate()
        {
            Created = DateTime.Now;
            LastUpd = DateTime.Now;
        }
    }
}
