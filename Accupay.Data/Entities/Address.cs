using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("address")]
    public class Address : BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int? CreatedBy { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? LastUpd { get; set; }

        public int? LastUpdBy { get; set; }

        public string StreetAddress1 { get; set; }
        public string StreetAddress2 { get; set; }
        public string Barangay { get; set; }
        public string CityTown { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string FullAddress
        {
            get
            {
                var address = (string.IsNullOrWhiteSpace(StreetAddress1) ? "" : StreetAddress1 + ", ") +
                        (string.IsNullOrWhiteSpace(StreetAddress2) ? "" : StreetAddress2 + ", ") +
                        (string.IsNullOrWhiteSpace(Barangay) ? "" : (Barangay.ToLower().IndexOf("city") > 0 ? Barangay : "Brgy. " + Barangay) + ", ") +
                        (string.IsNullOrWhiteSpace(CityTown) ? "" : (CityTown.ToLower().IndexOf("city") > 0 ? CityTown : CityTown + " city") + ", ") +
                        (string.IsNullOrWhiteSpace(Country) ? "" : Country + ", ") +
                        (string.IsNullOrWhiteSpace(State) ? "" : State + ", ") +
                        (string.IsNullOrWhiteSpace(ZipCode) ? "" : ZipCode + ", ")
                        ;

                var find = ",";

                int place = address.LastIndexOf(find);

                if (place == -1)
                    return address;

                string result = address.Remove(place, find.Length);
                return result;
            }
        }
    }
}