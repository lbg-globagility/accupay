using AccuPay.CrystalReportsWeb.Services;
using System;
using System.Net.Http;
using System.Web.Http;

namespace AccuPay.CrystalReportsWeb.Controllers
{
    /// <summary>
    /// Reports API endpoints returning pdf files.
    /// </summary>
    [RoutePrefix("api/reports")]
    public class ReportsController : ApiReportController
    {
        private readonly ReportingService _reportingService;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ReportsController(ReportingService reportingService)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _reportingService = reportingService;
        }

        /// <summary>
        /// Returns a pdf file containing all of the employee payslips of the selected pay period.
        /// </summary>
        /// <param name="payPeriodId">The pay period ID which determines the organization and cutoff dates of the report.</param>
        /// <returns></returns>
        [Route("payslip/{payPeriodId}")]
        public HttpResponseMessage GetPayslip(int payPeriodId)
        {
            string pdfFullPath = _reportingService.GeneratePayslip(payPeriodId);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the SSS Monthly report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="month">The month of the report.</param>
        /// <param name="year">The year of the report.</param>
        /// <returns></returns>
        [Route("sss-report/{organizationId}/{month}/{year}")]
        public HttpResponseMessage GetSSSMonthlyReport(int organizationId, int month, int year)
        {
            var dateMonth = new DateTime(year, month, 1);

            string pdfFullPath = _reportingService.GenerateSSSMonthlyReport(organizationId, dateMonth);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the PhilHealth Monthly report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="month">The month of the report.</param>
        /// <param name="year">The year of the report.</param>
        /// <returns></returns>
        [Route("philhealth-report/{organizationId}/{month}/{year}")]
        public HttpResponseMessage GetPhilHealthMonthlyReport(int organizationId, int month, int year)
        {
            var dateMonth = new DateTime(year, month, 1);

            string pdfFullPath = _reportingService.GeneratePhilHealthMonthlyReport(organizationId, dateMonth);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the PagIBIG Contribution report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="month">The month of the report.</param>
        /// <param name="year">The year of the report.</param>
        /// <returns></returns>
        [Route("pagibig-report/{organizationId}/{month}/{year}")]
        public HttpResponseMessage GetPagIBIGMonthlyReport(int organizationId, int month, int year)
        {
            var dateMonth = new DateTime(year, month, 1);

            string pdfFullPath = _reportingService.GeneratePagIBIGMonthlyReport(organizationId, dateMonth);

            return PdfReportResult(pdfFullPath);
        }
    }
}