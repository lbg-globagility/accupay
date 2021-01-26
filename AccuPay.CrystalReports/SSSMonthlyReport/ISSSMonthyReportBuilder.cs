using System;

namespace AccuPay.CrystalReports
{
    public interface ISSSMonthyReportBuilder : IBaseReportBuilder
    {
        SSSMonthyReportBuilder CreateReportDocument(int organizationId, DateTime dateMonth);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
