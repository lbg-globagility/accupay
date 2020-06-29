using AccuPay.CrystalReports.PhilHealthMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class PhilHealthMonthlyReportCreator
    {
        private readonly PhilHealthMonthlyReportDataService _dataService;
        private ReportClass _reportDocument;

        public PhilHealthMonthlyReportCreator(PhilHealthMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public PhilHealthMonthlyReportCreator CreateReportDocument(int organizationId, DateTime date)
        {
            _reportDocument = new Phil_Health_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
            objText.Text = "For the month of " + date_from;

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}