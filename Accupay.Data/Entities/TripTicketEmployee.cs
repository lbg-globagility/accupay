using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("tripticketemployee")]
    public class TripTicketEmployee
    {
        [Key]
        public int? RowID { get; set; }

        public int? OrganizationID { get; set; }

        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public int? TripTicketID { get; set; }

        public int? EmployeeID { get; set; }

        public int NoOfTrips { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("TripTicketID")]
        public virtual TripTicket TripTicket { get; set; }

        public string FullName => Employee.FullName;

        public string PositionName => Employee.Position.Name;
    }
}
