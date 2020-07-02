using AccuPay.CrystalReports;
using System;
using System.IO;

namespace AccuPay.CrystalReportsWeb.Services
{
    public class ReportingService : IControllerService
    {
        private readonly PayslipBuilder _payslipCreator;
        private readonly SSSMonthyReportBuilder _sSSMonthyReportBuilder;
        private readonly PhilHealthMonthlyReportBuilder _philHealthMonthlyReportBuilder;
        private readonly PagIBIGMonthlyReportBuilder _pagIBIGMonthlyReportBuilder;
        private readonly LoanSummaryByTypeReportBuilder _loanSummaryByTypeReportBuilder;
        private readonly LoanSummaryByEmployeeReportBuilder _loanSummaryByEmployeeReportBuilder;

        public ReportingService(
            PayslipBuilder payslipCreator,
            SSSMonthyReportBuilder sSSMonthyReportCreator,
            PhilHealthMonthlyReportBuilder philHealthMonthlyReportBuilder,
            PagIBIGMonthlyReportBuilder pagIBIGMonthlyReportBuilder,
             LoanSummaryByTypeReportBuilder loanSummaryByTypeReportBuilder,
             LoanSummaryByEmployeeReportBuilder loanSummaryByEmployeeReportBuilder)
        {
            _payslipCreator = payslipCreator;
            _sSSMonthyReportBuilder = sSSMonthyReportCreator;
            _philHealthMonthlyReportBuilder = philHealthMonthlyReportBuilder;
            _pagIBIGMonthlyReportBuilder = pagIBIGMonthlyReportBuilder;
            _loanSummaryByTypeReportBuilder = loanSummaryByTypeReportBuilder;
            _loanSummaryByEmployeeReportBuilder = loanSummaryByEmployeeReportBuilder;
        }

        public string GeneratePayslip(int payPeriodId)
        {
            string pdfFullPath = Path.GetTempFileName();

            _payslipCreator
                .CreateReportDocument(payPeriodId: payPeriodId)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }

        public string GenerateSSSMonthlyReport(int organizationId, DateTime dateMonth)
        {
            string pdfFullPath = Path.GetTempFileName();

            _sSSMonthyReportBuilder
                .CreateReportDocument(organizationId, dateMonth)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }

        public string GeneratePhilHealthMonthlyReport(int organizationId, DateTime dateMonth)
        {
            string pdfFullPath = Path.GetTempFileName();

            _philHealthMonthlyReportBuilder
                .CreateReportDocument(organizationId, dateMonth)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }

        public string GeneratePagIBIGContributionReport(int organizationId, DateTime dateMonth)
        {
            string pdfFullPath = Path.GetTempFileName();

            _pagIBIGMonthlyReportBuilder
                .CreateReportDocument(organizationId, dateMonth)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }

        public string GenerateLoanByTypeReport(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            string pdfFullPath = Path.GetTempFileName();

            _loanSummaryByTypeReportBuilder
                .CreateReportDocument(organizationId, dateFrom, dateTo)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }

        public string GenerateLoanByEmployeeReport(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            string pdfFullPath = Path.GetTempFileName();

            _loanSummaryByEmployeeReportBuilder
                .CreateReportDocument(organizationId, dateFrom, dateTo)
                .GeneratePDF(pdfFullPath);

            return pdfFullPath;
        }
    }
}