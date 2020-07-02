using AccuPay.CrystalReports.TaxMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class TaxMonthlyReportBuilder : BaseReportBuilder, IPdfGenerator
    {
        private readonly TaxMonthlyReportDataService _dataService;

        public TaxMonthlyReportBuilder(TaxMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public TaxMonthlyReportBuilder CreateReportDocument(int organizationId, int month, int year)
        {
            _reportDocument = new Tax_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, month, year));

            var dateMonth = new DateTime(year, month, 1);

            var objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];
            objText.Text = "For the month of  " + dateMonth.ToString("MMMM yyyy");

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
