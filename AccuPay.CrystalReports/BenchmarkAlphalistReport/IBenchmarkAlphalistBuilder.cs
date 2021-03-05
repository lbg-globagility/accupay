using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public interface IBenchmarkAlphalistBuilder
    {
        Task<BenchmarkAlphalistBuilder> CreateReportDocument(int organizationId, int year);

        BaseReportBuilder GeneratePDF(string pdfFullPath);
    }
}
