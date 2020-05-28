using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Calendars
{
    public class CreateCalendarDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CopiedCalendarId { get; set; }
    }
}
