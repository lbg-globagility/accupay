using AccuPay.CrystalReports.Payslip;
using AccuPay.Data;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace AccuPay.CrystalReportsWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var container = new UnityContainer();

            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();

            builder
                .UseMySql("server=localhost;userid=root;password=globagility;database=accupaydb_cinema2k;")
                .EnableSensitiveDataLogging()
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            container.RegisterType<PayrollContext>(new HierarchicalLifetimeManager(),
                new InjectionConstructor(builder.Options));

            container.RegisterType<AddressRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<OrganizationRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<PayPeriodRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<PayslipDataService>(new HierarchicalLifetimeManager());
            container.RegisterType<SystemOwnerService>(new HierarchicalLifetimeManager());
            container.RegisterType<PayslipCreator>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}