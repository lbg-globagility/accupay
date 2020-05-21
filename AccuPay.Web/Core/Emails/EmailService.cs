using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public class EmailService
    {
        private readonly IEmailSender _sender;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IEmailSender sender, ILogger<EmailService> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public async Task Send(Email email)
        {
            try
            {
                await _sender.Send(email);

                _logger.LogInformation("Email was sent to SMTP server");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email failed sending to SMTP server");
            }
        }
    }
}
