using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public class EmailService
    {
        private readonly IEmailSender sender;
        private readonly ILogger<EmailService> logger;

        public EmailService(IEmailSender sender, ILogger<EmailService> logger)
        {
            this.sender = sender;
            this.logger = logger;
        }

        public async Task Send(Email email)
        {
            try
            {
                await this.sender.Send(email);

                this.logger.LogInformation("Email was sent to SMTP server");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Email failed sending to SMTP server");
            }
        }
    }
}
