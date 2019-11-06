using GlobagilityShared.EmailSender;
using System.Configuration;

namespace AccupayWindowsService
{
    public class EmailConfig : IEmailConfig
    {
        public string SmtpHost => ConfigurationManager.AppSettings["SmtpHost"];

        public int SmtpPort
        {
            get
            {
                var port = ConfigurationManager.AppSettings["SmtpPort"];

                int num = 0;

                if (int.TryParse(port, out num))
                {
                    return num;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string SmtpSender => ConfigurationManager.AppSettings["SmtpSender"];
        public string SmtpDisplayName => ConfigurationManager.AppSettings["SmtpDisplayName"];
        public string SmtpPassword => ConfigurationManager.AppSettings["SmtpPassword"];
    }
}