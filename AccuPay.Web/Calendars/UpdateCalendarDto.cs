using System.ComponentModel.DataAnnotations;

namespace AccuPay.Web.Calendars
{
    public class UpdateCalendarDto
    {
        [Required]
        public string Name { get; set; }
    }
}
