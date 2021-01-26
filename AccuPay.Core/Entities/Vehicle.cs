using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("vehicle")]
    public class Vehicle
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

        public string PlateNo { get; set; }

        public string BodyNo { get; set; }

        public string TruckType { get; set; }

        public Vehicle()
        {
            Created = DateTime.Now;
        }

        public static Vehicle NewVehicle(int organizationId, int userId)
        {
            return new Vehicle() { OrganizationID = organizationId, CreatedBy = userId };
        }
    }
}