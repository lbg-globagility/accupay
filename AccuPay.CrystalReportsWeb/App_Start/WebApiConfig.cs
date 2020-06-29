using AccuPay.CrystalReports.Payslip;
using AccuPay.CrystalReportsWeb.DependencyHelper;
using AccuPay.Data;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            DependencyConfig.Register(config);

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