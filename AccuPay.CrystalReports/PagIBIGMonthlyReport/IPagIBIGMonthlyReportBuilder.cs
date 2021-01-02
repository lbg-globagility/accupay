using System;

namespace AccuPay.CrystalReports
{
    public interface IPagIBIGMonthlyReportBuilder : IBaseReportBuilder
    {
        PagIBIGMonthlyReportBuilder CreateReportDocument(int organizationId, DateTime date);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
