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
        public HttpResponseMessage GetPagIBIGContributionReport(int organizationId, int month, int year)
        {
            var dateMonth = new DateTime(year, month, 1);

            string pdfFullPath = _reportingService.GeneratePagIBIGContributionReport(organizationId, dateMonth);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the Loan by type report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="dateFrom">The start date of the report.</param>
        /// <param name="dateTo">The end date of the report.</param>
        /// <param name="isPerPage">The report is generated per page.</param>
        /// <returns></returns>
        [Route("loanbytype-report/{organizationId}/{dateFrom}/{dateTo}/{isPerPage}")]
        public HttpResponseMessage GetLoanByTypeReport(int organizationId, DateTime dateFrom, DateTime dateTo, bool isPerPage)
        {
            string pdfFullPath = _reportingService.GenerateLoanByTypeReport(organizationId, dateFrom, dateTo, isPerPage);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the Loan by Employee report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="dateFrom">The start date of the report.</param>
        /// <param name="dateTo">The end date of the report.</param>
        /// <returns></returns>
        [Route("loanbyemployee-report/{organizationId}/{dateFrom}/{dateTo}")]
        public HttpResponseMessage GetLoanByEmployeeReport(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            string pdfFullPath = _reportingService.GenerateLoanByEmployeeReport(organizationId, dateFrom, dateTo);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the Tax report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="month">The month of the report.</param>
        /// <param name="year">The year of the report.</param>
        /// <returns></returns>
        [Route("tax-report/{organizationId}/{month}/{year}")]
        public HttpResponseMessage GetTaxReport(int organizationId, int month, int year)
        {
            string pdfFullPath = _reportingService.GenerateTaxReport(organizationId, month, year);

            return PdfReportResult(pdfFullPath);
        }

        /// <summary>
        /// Returns a pdf file containing the 13th month report of the selected month and year.
        /// </summary>
        /// <param name="organizationId">The ID of the organization that the report will be based on.</param>
        /// <param name="dateFrom">The start date of the report.</param>
        /// <param name="dateTo">The end date of the report.</param>
        /// <returns></returns>
        [Route("thirteenth-month-report/{organizationId}/{dateFrom}/{dateTo}")]
        public HttpResponseMessage GetThirteenthMonthReport(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            string pdfFullPath = _reportingService.GenerateThirteenthMonthReport(organizationId, dateFrom, dateTo);

            return PdfReportResult(pdfFullPath);
        }
    }
}