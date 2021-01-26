using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.PagIBIGMonthlyReport;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class PagIBIGMonthlyReportBuilder : BaseReportBuilder, IPdfGenerator, IPagIBIGMonthlyReportBuilder
    {
        private readonly IPagIBIGMonthlyReportDataService _dataService;

        public PagIBIGMonthlyReportBuilder(IPagIBIGMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public PagIBIGMonthlyReportBuilder CreateReportDocument(int organizationId, DateTime date)
        {
            _reportDocument = new Pagibig_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
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
