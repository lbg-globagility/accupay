using Microsoft.AspNetCore.Http;

namespace AccuPay.Web.Users.Models
{
    public class UserImageDto
    {
        public IFormFile? Image { get; set; }
    }
}
