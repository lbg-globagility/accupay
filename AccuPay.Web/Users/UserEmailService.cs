using AccuPay.Data.Entities;
using AccuPay.Web.Core.Emails;
using AccuPay.Web.Core.Views;
using AccuPay.Web.Users;
using AccuPay.Web.Views.Emails;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Notisphere.Users.Services
{
    public class UserEmailService
    {
        private readonly ViewRenderService _renderService;
        private readonly UserTokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public UserEmailService(
            ViewRenderService renderService,
            UserTokenService tokenService,
            IConfiguration configuration,
            EmailService emailService)
        {
            _renderService = renderService;
            _tokenService = tokenService;
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task SendInvitation(AspNetUser user)
        {
            var token = _tokenService.CreateRegistrationToken(user);

            var domain = _configuration["App:Domain"];

            var model = new InviteModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Url = $"{domain}/register?token={token}"
            };

            var content = await _renderService.RenderHtml("Emails/Invite", model);

            var email = new Email("[AccuPay] You're invited to join AccuPay", user.Email);
            email.Html = content;

            await _emailService.Send(email);
        }
    }
}
