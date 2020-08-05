using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("tripticket")]
    public class TripTicket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? VehicleID { get; set; }

        public int? RouteID { get; set; }

        public string TicketNo { get; set; }

        public DateTime? Date { get; set; }
        [Column("Date")]
        public DateTime? TripDate { get; set; }

        public TimeSpan? TimeFrom { get; set; }

        public TimeSpan? TimeTo { get; set; }

        public TimeSpan? TimeDispatched { get; set; }

        public string Guide { get; set; }

        public bool IsSpecialOperations { get; set; }

        public virtual ICollection<TripTicketEmployee> Employees { get; set; }

        [ForeignKey("VehicleID")]
        public virtual Vehicle Vehicle { get; set; }

        [ForeignKey("RouteID")]
        public virtual Route Route { get; set; }

        public string RouteDescription => Route?.Description;

        public TripTicket()
        {
            Created = DateTime.Now;
        }

        public string VehiclePlateNo => Vehicle?.PlateNo;

        public virtual ICollection<TripTicketEmployee> TripTicketEmployees { get; set; }

        public static TripTicket NewTripTicket(int organizationId, int userId)
        {
            return new TripTicket() { OrganizationID = organizationId, CreatedBy = userId };
        }
    }
}