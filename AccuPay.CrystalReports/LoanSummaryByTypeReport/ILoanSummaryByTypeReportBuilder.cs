using System;

namespace AccuPay.CrystalReports
{
    public interface ILoanSummaryByTypeReportBuilder : IBaseReportBuilder
    {
        LoanSummaryByTypeReportBuilder CreateReportDocument(int organizationId, DateTime dateFrom, DateTime dateTo);

        LoanSummaryByTypeReportBuilder CreateReportDocumentPerPage(int organizationId, DateTime dateFrom, DateTime dateTo);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
