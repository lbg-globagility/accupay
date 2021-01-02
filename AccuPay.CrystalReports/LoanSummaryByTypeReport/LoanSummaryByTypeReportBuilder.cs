using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.LoanSummaryByTypeReport;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class LoanSummaryByTypeReportBuilder : BaseReportBuilder, IPdfGenerator, ILoanSummaryByTypeReportBuilder
    {
        private readonly ILoanSummaryByTypeReportDataService _dataService;

        public LoanSummaryByTypeReportBuilder(ILoanSummaryByTypeReportDataService dataService)
        {
            _dataService = dataService;
        }

        public LoanSummaryByTypeReportBuilder CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new LoanReportByType();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text14"];

            var dateFromTitle = dateFrom.ToString("MMMM d, yyyy");
            var dateTotTitle = dateTo.ToString("MMMM d, yyyy");

            objText.Text = $"For the period of {dateFromTitle} to {dateTotTitle}";

            return this;
        }

        public LoanSummaryByTypeReportBuilder CreateReportDocumentPerPage(int organizationId, DateTime dateFrom, DateTime dateTo)
        {
            _reportDocument = new LoanReportByTypePerPage();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, dateFrom, dateTo));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["Text14"];

            var dateFromTitle = dateFrom.ToString("MMMM d, yyyy");
            var dateTotTitle = dateTo.ToString("MMMM d, yyyy");

            objText.Text = $"For the period of {dateFromTitle} to {dateTotTitle}";

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
