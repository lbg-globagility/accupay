using AccuPay.Web.Core.Auth;
using AccuPay.Web.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AccuPay.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ReportsControllerBase
    {
        public const string DateFormat = "yyyy-MM-dd";
        private readonly ReportService _service;

        public ReportsController(ICurrentUser currentUser, IConfiguration configuration, ReportService service) : base(currentUser, configuration)
        {
            _service = service;
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

        [HttpGet("loanbytype-report/{dateFrom}/{dateTo}/{isPerPage}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult> GetLoanByTypeReport(DateTime dateFrom, DateTime dateTo, bool isPerPage)
        {
            var path = $"loanbyType-report/{_currentUser.OrganizationId}/{dateFrom.ToString(DateFormat)}/{dateTo.ToString(DateFormat)}/{isPerPage}";
            return await GetPDF(path, "loan-by-type-report.pdf");
        }

        [HttpGet("loanbyemployee-report/{dateFrom}/{dateTo}")]
        [Permission(PermissionTypes.LoanRead)]
        public async Task<ActionResult> GetLoanByEmployeeReport(DateTime dateFrom, DateTime dateTo)
        {
            var path = $"loanbyemployee-report/{_currentUser.OrganizationId}/{dateFrom.ToString(DateFormat)}/{dateTo.ToString(DateFormat)}";
            return await GetPDF(path, "loan-by-employee-report.pdf");
        }

        [HttpGet("tax-report/{month}/{year}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetTaxReport(int month, int year)
        {
            var path = $"tax-report/{_currentUser.OrganizationId}/{month}/{year}";
            return await GetPDF(path, "tax-report.pdf");
        }

        [HttpGet("thirteenth-month-report/{dateFrom}/{dateTo}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetThirteenthMonthReport(DateTime dateFrom, DateTime dateTo)
        {
            var path = $"thirteenth-month-report/{_currentUser.OrganizationId}/{dateFrom.ToString(DateFormat)}/{dateTo.ToString(DateFormat)}";
            return await GetPDF(path, "thirteenth-month-report.pdf");
        }

        [HttpGet("payroll-summary/{payPeriodFromId}/{payPeriodToId}")]
        [Permission(PermissionTypes.PayPeriodRead)]
        public async Task<ActionResult> GetPayrollSummary(
            int payPeriodFromId,
            int payPeriodToId,
            bool keepInOneSheet = true,
            bool hideEmptyColumns = true,
            string salaryDistributionType = "Cash",
            bool isActual = false)
        {
            var fileName = await _service.GetPayrollSummary(
                keepInOneSheet: keepInOneSheet,
                hideEmptyColumns: hideEmptyColumns,
                payPeriodFromId: payPeriodFromId,
                payPeriodToId: payPeriodToId,
                salaryDistributionType: salaryDistributionType,
                isActual: isActual);

            var result = PhysicalFile(fileName, "application/vnd.ms-excel");

            Response.Headers["Content-Disposition"] = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "Payroll Summary"
            }.ToString();

            return result;
        }
    }
}
