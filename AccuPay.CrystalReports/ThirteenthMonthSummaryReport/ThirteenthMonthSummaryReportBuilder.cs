using AccuPay.CrystalReports.ThirteenthMonthSummaryReport;
using AccuPay.Core.ValueObjects;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

namespace AccuPay.CrystalReports
{
    public class ThirteenthMonthSummaryReportBuilder : BaseReportBuilder, IPdfGenerator
    {
        public ThirteenthMonthSummaryReportBuilder CreateReportDocument(DataTable data, TimePeriod timePeriod)
        {
            _reportDocument = new ThirteenthMonthSummary();

            var objText = (TextObject)_reportDocument.ReportDefinition.Sections[1].ReportObjects["PeriodDate"];

            objText.Text = string.Concat(
                "Salary from ",
                timePeriod.Start.ToShortDateString(),
                " to ",
                timePeriod.End.ToShortDateString());

            _reportDocument.SetDataSource(data);

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
