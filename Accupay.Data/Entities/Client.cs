using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("client")]
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string TradeName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPerson { get; set; }
    }
}
