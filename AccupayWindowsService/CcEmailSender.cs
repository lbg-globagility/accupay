using GlobagilityShared.EmailSender;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace AccupayWindowsService
{
    /// <summary>
    /// SMTP sender that supports CC recipients.
    /// GlobagilityShared.EmailSender only supports a single "To" address, so this is used when CC addresses are configured.
    /// </summary>
    public class CcEmailSender
    {
        private readonly IEmailConfig _config;

        public CcEmailSender(IEmailConfig config)
        {
            _config = config;
        }

        public void SendEmail(
            string to,
            IEnumerable<string> ccAddresses,
            string subject,
            string body,
            string[] attachments)
        {
            using (var client = CreateSmtpClient())
            using (var message = new MailMessage())
            {
                message.From = string.IsNullOrWhiteSpace(_config.SmtpDisplayName)
                    ? new MailAddress(_config.SmtpSender)
                    : new MailAddress(_config.SmtpSender, _config.SmtpDisplayName);

                message.To.Add(to);

                if (ccAddresses != null)
                {
                    foreach (var cc in ccAddresses)
                    {
                        message.CC.Add(cc);
                    }
                }

                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = false;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        message.Attachments.Add(new Attachment(attachment));
                    }
                }

                client.Send(message);
            }
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient()
            {
                Host = _config.SmtpHost,
                Port = _config.SmtpPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_config.SmtpSender, _config.SmtpPassword)
            };
        }
    }
}
