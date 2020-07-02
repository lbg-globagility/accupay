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

        [HttpGet("philhealth-report/{month}/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPhilHeathReport(int month, int year)
        {
            var path = $"philhealth-report/{_currentUser.OrganizationId}/{month}/{year}";
            return await GetPDF(path, "philhealth-report.pdf");
        }

        [HttpGet("pagibig-report/{month}/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPagIBIGReport(int month, int year)
        {
            var path = $"pagibig-report/{_currentUser.OrganizationId}/{month}/{year}";
            return await GetPDF(path, "pagibig-report.pdf");
        }

        [HttpGet("loanbytype-report/{monthFrom}/{dayFrom}/{yearFrom}/{monthTo}/{dayTo}/{yearTo}/{isPerPage}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult> GetLoanByTypeReport(int monthFrom, int dayFrom, int yearFrom, int monthTo, int dayTo, int yearTo, bool isPerPage)
        {
            var path = $"loanbyType-report/{_currentUser.OrganizationId}/{monthFrom}/{dayFrom}/{yearFrom}/{monthTo}/{dayTo}/{yearTo}/{isPerPage}";
            return await GetPDF(path, "loan-by-type-report.pdf");
        }

        [HttpGet("loanbyemployee-report/{monthFrom}/{dayFrom}/{yearFrom}/{monthTo}/{dayTo}/{yearTo}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult> GetLoanByEmployeeReport(int monthFrom, int dayFrom, int yearFrom, int monthTo, int dayTo, int yearTo, bool isPerPage)
        {
            var path = $"loanbyemployee-report/{_currentUser.OrganizationId}/{monthFrom}/{dayFrom}/{yearFrom}/{monthTo}/{dayTo}/{yearTo}";
            return await GetPDF(path, "loan-by-employee-report.pdf");
        }
    }
}
