using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("tripticketemployee")]
    public class TripTicketEmployee
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

        public int? TripTicketID { get; set; }

        public int? EmployeeID { get; set; }

        public int NoOfTrips { get; set; }

        [ForeignKey("EmployeeID")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("TripTicketID")]
        public virtual TripTicket TripTicket { get; set; }

        public string FullName => Employee.FullName;

        public string PositionName => Employee.Position?.Name;

        public bool IsNew => (RowID ?? 0) <= 0;

        public static TripTicketEmployee NewTripTicketEmployee(int organizationId, int userId)
        {
            return new TripTicketEmployee() { OrganizationID = organizationId, CreatedBy = userId };
        }
    }
}
