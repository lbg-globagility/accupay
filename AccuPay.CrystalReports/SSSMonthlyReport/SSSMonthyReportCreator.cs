using AccuPay.CrystalReports.SSSMonthlyReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class SSSMonthyReportCreator
    {
        private readonly SSSMonthlyReportDataService _dataService;
        private ReportClass _reportDocument;

        public SSSMonthyReportCreator(SSSMonthlyReportDataService dataService)
        {
            _dataService = dataService;
        }

        public SSSMonthyReportCreator CreateReportDocument(int organizationId, DateTime date)
        {
            _reportDocument = new SSS_Monthly_Report();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text2"];

            var date_from = date.ToString("MMMM  yyyy");
            objText.Text = "for the month of " + date_from;

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}