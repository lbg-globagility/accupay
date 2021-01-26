namespace AccuPay.CrystalReports
{
    public interface ITaxMonthlyReportBuilder : IBaseReportBuilder
    {
        TaxMonthlyReportBuilder CreateReportDocument(int organizationId, int month, int year);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
