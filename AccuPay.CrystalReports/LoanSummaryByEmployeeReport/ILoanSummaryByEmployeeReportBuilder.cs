using System;

namespace AccuPay.CrystalReports
{
    public interface ILoanSummaryByEmployeeReportBuilder : IBaseReportBuilder
    {
        LoanSummaryByEmployeeReportBuilder CreateReportDocument(int organizationId, DateTime date_from, DateTime date_to);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
