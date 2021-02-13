using AccuPay.Core.Interfaces;
using AccuPay.CrystalReports.BenchmarkAlphalistReport;
using CrystalDecisions.CrystalReports.Engine;
using System.Threading.Tasks;

namespace AccuPay.CrystalReports
{
    public class BenchmarkAlphalistBuilder : BaseReportBuilder, IPdfGenerator, IBenchmarkAlphalistBuilder
    {
        private readonly IBenchmarkAlphalistReportDataService _dataService;

        public BenchmarkAlphalistBuilder(IBenchmarkAlphalistReportDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<BenchmarkAlphalistBuilder> CreateReportDocument(int organizationId, int year)
        {
            _reportDocument = new BenchmarkAlphalist();

            _reportDocument.SetDataSource(await _dataService.GetData(organizationId, year));

            TextObject objText = (TextObject)_reportDocument.ReportDefinition.Sections[2].ReportObjects["YearHeader"];
            objText.Text = year.ToString();

            return this;
        }

        public BaseReportBuilder GeneratePDF(string pdfFullPath)
        {
            ExportPDF(pdfFullPath);

            return this;
        }
    }
}
