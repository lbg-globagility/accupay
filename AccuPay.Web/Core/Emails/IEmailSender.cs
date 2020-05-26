using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public interface IEmailSender
    {
        Task Send(Email email);
    }
}
