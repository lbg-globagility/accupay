using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.LoanSummaryByEmployeeReport;
using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AccuPay.CrystalReports
{
    public class LoanSummaryByEmployeeReportBuilder : BaseReportBuilder, IPdfGenerator, ILoanSummaryByEmployeeReportBuilder
    {
        private readonly ILoanSummaryByEmployeeReportDataService _dataService;

        public LoanSummaryByEmployeeReportBuilder(ILoanSummaryByEmployeeReportDataService dataService)
        {
            _dataService = dataService;
        }

        public LoanSummaryByEmployeeReportBuilder CreateReportDocument(int organizationId, DateTime date_from, DateTime date_to)
        {
            _reportDocument = new LoanReports();

            _reportDocument.SetDataSource(_dataService.GetData(organizationId, date_from, date_to));

            TextObject objText = null/* TODO Change to default(_) if this is not a reference type */;

            objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["PeriodDate"];

            objText.Text = string.Concat("for the period of ", date_from.ToShortDateString(), " to ", date_to.ToShortDateString());

            objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["txtOrganizationName"];

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
