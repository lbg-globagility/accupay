using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.SSSMonthlyReport;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class SSSMonthyReportBuilder : BaseReportBuilder, IPdfGenerator, ISSSMonthyReportBuilder
    {
        private readonly ISSSMonthlyReportDataService _dataService;

        public SSSMonthyReportBuilder(ISSSMonthlyReportDataService dataService)
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
