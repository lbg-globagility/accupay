using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("tripticket")]
    public class TripTicket
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? VehicleID { get; set; }

        public int? RouteID { get; set; }

        public string TicketNo { get; set; }

        public DateTime? TripDate { get; set; }

        public TimeSpan? TimeFrom { get; set; }

        public TimeSpan? TimeTo { get; set; }

        public TimeSpan? TimeDispatched { get; set; }

        public string Guide { get; set; }

        public bool IsSpecialOperations { get; set; }

        public TripTicket()
        {
            Created = DateTime.Now;
        }

        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }

        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }

        public string RouteDescription => Route?.Description;
    }
}
