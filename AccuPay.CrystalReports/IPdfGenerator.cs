namespace AccuPay.CrystalReports
{
    public interface IPdfGenerator
    {
        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}