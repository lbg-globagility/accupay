using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public class NullEmailSender : IEmailSender
    {
        public Task Send(Email email)
        {
            return Task.CompletedTask;
        }
    }
}
