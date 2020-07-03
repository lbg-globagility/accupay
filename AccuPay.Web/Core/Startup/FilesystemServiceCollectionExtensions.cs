using AccuPay.Web.Core.Files;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AccuPay.Web.Core.Startup
{
    public static class FilesystemServiceCollectionExtensions
    {
        public static IServiceCollection AddFilesystem(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IFilesystem>(serviceProvider =>
            {
                switch (configuration["Filesystem:Type"])
                {
                    case "local":
                        return new LocalFilesystem(configuration["Filesystem:RootDirectory"]);

                    //case "s3":
                    //    return new S3Filesystem(configuration["Filesystem:Bucket"]);

                    default:
                        throw new Exception();
                }
            });

            return services;
        }
    }
}
