using AccuPay.Core.ValueObjects;
using System.Data;

namespace AccuPay.CrystalReports
{
    public interface IThirteenthMonthSummaryReportBuilder : IBaseReportBuilder
    {
        ThirteenthMonthSummaryReportBuilder CreateReportDocument(DataTable data, TimePeriod timePeriod);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
