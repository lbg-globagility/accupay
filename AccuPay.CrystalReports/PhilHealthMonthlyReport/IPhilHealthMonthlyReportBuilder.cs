using System;

namespace AccuPay.CrystalReports
{
    public interface IPhilHealthMonthlyReportBuilder : IBaseReportBuilder
    {
        PhilHealthMonthlyReportBuilder CreateReportDocument(int organizationId, DateTime date);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
