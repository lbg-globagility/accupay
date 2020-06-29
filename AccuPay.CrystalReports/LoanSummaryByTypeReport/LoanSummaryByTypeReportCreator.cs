using AccuPay.CrystalReports.LoanSummaryByTypeReport;
using AccuPay.Data.Services;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class LoanSummaryByTypeReportCreator
    {
        private readonly LoanSummaryByTypeReportDataService _dataService;
        private ReportClass _reportDocument;

        public LoanSummaryByTypeReportCreator(LoanSummaryByTypeReportDataService dataService)
        {
            _dataService = dataService;
        }

        public LoanSummaryByTypeReportCreator CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new LoanReportByType();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text14"];

            var dateFromTitle = dateFrom.ToString("MMMM d, yyyy");
            var dateTotTitle = dateTo.ToString("MMMM d, yyyy");

            objText.Text = $"For the period of {dateFromTitle} to {dateTotTitle}";

            return this;
        }

        public LoanSummaryByTypeReportCreator CreateReportDocumentPerPage(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new LoanReportByTypePerPage();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text14"];

            var dateFromTitle = dateFrom.ToString("MMMM d, yyyy");
            var dateTotTitle = dateTo.ToString("MMMM d, yyyy");

            objText.Text = $"For the period of {dateFromTitle} to {dateTotTitle}";

            return this;
        }

        public ReportClass GetReportDocument()
        {
            return _reportDocument;
        }
    }
}
