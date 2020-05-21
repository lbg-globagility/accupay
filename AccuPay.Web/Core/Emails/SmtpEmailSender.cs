using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _senderAddress;
        private readonly string _password;
        private readonly string _displayName;

        public SmtpEmailSender(string host, int port, string senderAddress, string password, string displayName)
        {
            _host = host;
            _port = port;
            _senderAddress = senderAddress;
            _password = password;
            _displayName = displayName;
        }

        public Task Send(Email email)
        {
            using (var client = CreateSmtpClient())
            using (var message = new MailMessage())
            {
                AddSenderAddress(message);
                AddRecipients(message, email.Recipients);

                message.Subject = email.Subject;

                AddTextVersion(message, email.Text);
                AddHtmlVersion(message, email.Html);

                client.Send(message);

                return Task.CompletedTask;
            }
        }

        private void AddSenderAddress(MailMessage message)
        {
            if (string.IsNullOrWhiteSpace(_displayName))
            {
                message.From = new MailAddress(_senderAddress);
            }
            else
            {
                message.From = new MailAddress(_senderAddress, _displayName);
            }
        }

        private void AddRecipients(MailMessage message, ICollection<string> emailAddresses)
        {
            foreach (var emailAddress in emailAddresses)
            {
                message.To.Add(emailAddress);
            }
        }

        private void AddTextVersion(MailMessage message, string content)
        {
            content = content ?? string.Empty;

            var contentType = new ContentType("text/plain");
            var view = AlternateView.CreateAlternateViewFromString(content, contentType);

            message.AlternateViews.Add(view);
        }

        private void AddHtmlVersion(MailMessage message, string content)
        {
            var contentType = new ContentType("text/html");
            var view = AlternateView.CreateAlternateViewFromString(content, contentType);

            message.AlternateViews.Add(view);
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient()
            {
                Host = _host,
                Port = _port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_senderAddress, _password)
            };
        }
    }
}
