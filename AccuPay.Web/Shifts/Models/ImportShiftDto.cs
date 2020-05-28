using Microsoft.AspNetCore.Http;

namespace AccuPay.Web.Shifts.Models
{
    public class ImportShiftDto
    {
        public IFormFile File { get; set; }
    }
}
