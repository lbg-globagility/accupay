using AccuPay.Web.AutoMapperProfile;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace AccuPay.Web
{
    public static class AutoMapperServiceCollectionExtensions
    {
        
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IMapper>(sp =>
            {
                // Resolve the logger factory from DI
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

                var config = new MapperConfiguration(cfg =>
                {
                    // Pass the logger factory to the configuration if needed
                    cfg.AddProfile(new AutoMapperProfileConfiguration());
                },loggerFactory);

                return config.CreateMapper();
            });

            return services;
        }
    }
}
