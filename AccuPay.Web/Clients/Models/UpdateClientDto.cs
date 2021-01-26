using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Clients
{
    public class UpdateClientDto
    {
        [Required]
        public string Name { get; set; }

        public string TradeName { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string ContactPerson { get; set; }
    }
}
