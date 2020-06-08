using AccuPay.Data;
using AccuPay.Data.Entities;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Core.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AccuPay.Web
{
    public static class AuthenticationServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthenticationService(this IServiceCollection services, JwtConfiguration configuration)
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

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = configuration.Issuer,
                        ValidAudience = configuration.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key))
                    };
                });

            services.AddAuthorization();

            //services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            //services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            return services;
        }
    }
}
