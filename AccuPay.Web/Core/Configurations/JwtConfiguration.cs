using Microsoft.Extensions.Configuration;

namespace AccuPay.Web.Core.Configurations
{
    public class JwtConfiguration
    {
        private readonly IConfiguration _configuration;

        public JwtConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Audience => _configuration["Jwt:Audience"];

        public string Issuer => _configuration["Jwt:Issuer"];

        public string Key => _configuration["Jwt:Key"];
    }
}
