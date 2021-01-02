using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.PhilHealthMonthlyReport;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class PhilHealthMonthlyReportBuilder : BaseReportBuilder, IPdfGenerator, IPhilHealthMonthlyReportBuilder
    {
        private readonly IPhilHealthMonthlyReportDataService _dataService;

        public PhilHealthMonthlyReportBuilder(IPhilHealthMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public PhilHealthMonthlyReportBuilder CreateReportDocument(int organizationId, DateTime date)
        {
            _reportDocument = new Phil_Health_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
            objText.Text = "For the month of " + date_from;

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
