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

        public ReportingService(
            PayslipBuilder payslipCreator,
            SSSMonthyReportBuilder sSSMonthyReportCreator,
            PhilHealthMonthlyReportBuilder philHealthMonthlyReportBuilder)
        {
            _payslipCreator = payslipCreator;
            _sSSMonthyReportBuilder = sSSMonthyReportCreator;
            _philHealthMonthlyReportBuilder = philHealthMonthlyReportBuilder;
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
    }
}