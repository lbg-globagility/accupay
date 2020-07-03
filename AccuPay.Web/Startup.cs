using AccuPay.Web.Core.Configurations;
using AccuPay.Web.Core.Json;
using AccuPay.Web.Core.Middlewares;
using AccuPay.Web.Core.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Notisphere.Core.Startup;
using System;
using System.IO;
using System.Reflection;

namespace AccuPay.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDatabase(Configuration);
            services.AddAccuPayCoreServices();
            services.AddAuthenticationService(new JwtConfiguration(Configuration));
            services.AddWebServices();
            services.AddEmailService(new EmailConfiguration(Configuration));
            services.AddFilesystem(Configuration);

            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(t => t.FullName);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseReDoc(a => a.SpecUrl = "/swagger/v1/swagger.json");
            }

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                // Prevent caching of index.html
                OnPrepareResponse = context =>
                {
                    if (context.File.Name == "index.html")
                    {
                        context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                        context.Context.Response.Headers.Add("Pragma", "no-cache");
                        context.Context.Response.Headers.Add("Expires", "0");
                    }
                }
            });

            // Only redirect to front-end assets if request url doesn't start with "/api"
            app.MapWhen(
                x => !x.Request.Path.Value.StartsWith("/api"),
                builder => builder.UseSpa(spa => { }));

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorLoggingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
