using AccuPay.Data;
using AccuPay.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<AspNetUser, AspNetRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                })
               .AddEntityFrameworkStores<PayrollContext>()
               .AddDefaultTokenProviders();

            services.AddAuthorization();

            return services;
        }
    }
}
