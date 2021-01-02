using CrystalDecisions.CrystalReports.Engine;

namespace AccuPay.CrystalReports
{
    public interface IBaseReportBuilder
    {
        string GetPDF();
        ReportClass GetReportDocument();
    }
}