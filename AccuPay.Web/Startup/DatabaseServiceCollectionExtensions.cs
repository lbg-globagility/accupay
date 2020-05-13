using AccuPay.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccuPay.Web
{
    public static class DatabaseServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PayrollContext>(options =>
            {
                options
                    .UseMySql(configuration.GetConnectionString("notispheredb"))
                    .EnableSensitiveDataLogging();
            });

            //services.AddSingleton(serviceProvider =>
            //{
            //    return new ConnectionFactory(configuration.GetConnectionString("notispheredb"));
            //});

            //SqlMapper.AddTypeHandler(new GuidToBigEndianBytesTypeHandler());
            //SqlMapper.RemoveTypeMap(typeof(Guid));
            //SqlMapper.RemoveTypeMap(typeof(Guid?));

            return services;
        }
    }
}
