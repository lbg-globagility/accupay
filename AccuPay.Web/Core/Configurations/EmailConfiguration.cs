using Microsoft.Extensions.Configuration;

namespace AccuPay.Web.Core.Configurations
{
    public class EmailConfiguration
    {
        private readonly IConfiguration _configuration;

        public EmailConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Type
        {
            get => _configuration["Email:Type"];
        }

        public string SesSender
        {
            get => _configuration["Email:Ses:Sender"];
        }

        public string SmtpHost
        {
            get => _configuration["Email:Smtp:Host"];
        }

        public int SmtpPort
        {
            get => _configuration.GetValue<int>("Email:Smtp:Port");
        }

        public string SmtpSender
        {
            get => _configuration["Email:Smtp:Sender"];
        }

        public string SmtpPassword
        {
            get => _configuration["Email:Smtp:Password"];
        }

        public string SmtpDisplayName
        {
            get => _configuration["Email:Smtp:DisplayName"];
        }

        public string FileDirectory
        {
            get => _configuration["Email:File:RootDirectory"];
        }
    }
}
