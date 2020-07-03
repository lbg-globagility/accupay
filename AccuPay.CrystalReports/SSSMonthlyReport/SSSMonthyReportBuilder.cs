using AccuPay.CrystalReports.SSSMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class SSSMonthyReportBuilder : BaseReportBuilder, IPdfGenerator
    {
        private readonly SSSMonthlyReportDataService _dataService;

        public SSSMonthyReportBuilder(SSSMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public SSSMonthyReportBuilder CreateReportDocument(int organizationId, DateTime dateMonth)
        {
            _reportDocument = new SSS_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateMonth));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = dateMonth.ToString("MMMM  yyyy");
            objText.Text = "for the month of " + date_from;

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}