using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ReportsControllerBase
    {
        public ReportsController(ICurrentUser currentUser, IConfiguration configuration) : base(currentUser, configuration)
        {
        }

        [HttpGet("sss-report/{month}/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPayslips(int month, int year)
        {
            var path = $"sss-report/{_currentUser.OrganizationId}/{month}/{year}";
            return await GetPDF(path, "sss-report.pdf");
        }
    }
}
