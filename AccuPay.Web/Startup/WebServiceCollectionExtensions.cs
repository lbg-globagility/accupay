using AccuPay.Web.Users;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class WebServiceCollectionExtensions
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services)
        {
            services.AddScoped<UserService>();

            return services;
        }
    }
}
