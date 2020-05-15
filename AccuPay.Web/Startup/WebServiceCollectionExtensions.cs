using Accupay.Web.Core.Configurations;
using AccuPay.Web.Account;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();
            services.AddScoped<AccountService>();
            services.AddScoped<AccountTokenService>();
            services.AddScoped<TokenService>();

            services.AddScoped<JwtConfiguration>();

            return services;
        }
    }
}
