using AccuPay.Web.Core.Configurations;
using AccuPay.Web.Core.Emails;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Notisphere.Core.Startup
{
    public static class EmailServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, EmailConfiguration config)
        {
            services.AddScoped<IEmailSender>(serviceProvider =>
            {
                return config.Type switch
                {
                    "smtp" => new SmtpEmailSender(config.SmtpHost,
                                                  config.SmtpPort,
                                                  config.SmtpSender,
                                                  config.SmtpPassword,
                                                  config.SmtpDisplayName),
                    "file" => new FileEmailSender(config.FileDirectory),
                    "null" => new NullEmailSender(),
                    _ => new NullEmailSender(),
                };
            });

            services.AddScoped<EmailService>();

            return services;
        }
    }
}
