using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AccuPay.Web.Core.Emails
{
    public class Email
    {
        public string Subject { get; set; }

        public string Text { get; set; }

        public string Html { get; set; }

        public ICollection<string> Recipients { get; set; }

        public ICollection<string> CcRecipients { get; set; }

        public ICollection<string> BccRecipients { get; set; }

        public Email()
        {
            Recipients = new Collection<string>();
            CcRecipients = new Collection<string>();
            BccRecipients = new Collection<string>();
        }

        public Email(string subject, string recipient)
            : this()
        {
            Subject = subject;
            AddRecipient(recipient);
        }

        public Email(string subject, ICollection<string> recipients)
            : this()
        {
            Subject = subject;

            foreach (var r in recipients)
            {
                AddRecipient(r);
            }
        }

        private void AddRecipient(string recipient)
        {
            if (HasRecipient(recipient))
            {
                return;
            }

            Recipients.Add(recipient);
        }

        private bool HasRecipient(string recipient)
        {
            return Recipients.Any(r => r == recipient);
        }
    }
}
