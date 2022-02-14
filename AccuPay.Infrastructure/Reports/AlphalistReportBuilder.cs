using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Reports;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Reports.Service;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports
{
    public class AlphalistReportBuilder : IAlphalistReportBuilder
    {
        private readonly IAlphaListReportDataService _alphaListReportDataService;

        public AlphalistReportBuilder(IAlphaListReportDataService alphaListReportDataService)
        {
            _alphaListReportDataService = alphaListReportDataService;
        }

        public async Task<string> GenerateReportAsync(
            int organizationId,
            bool actualSwitch,
            PayPeriod startPeriod,
            PayPeriod endPeriod,
            string saveFileDiretory)
        {
            var periodRange = new TimePeriod(startPeriod.PayFromDate,
                endPeriod.PayToDate);

            var models = await _alphaListReportDataService.GetData(organizationId: organizationId,
                actualSwitch: actualSwitch,
                startPeriod: startPeriod,
                endPeriod: endPeriod);

            var alphalistGenerator = new AlphalistGenerator(models: models,
                year: 0,
                dateFrom: periodRange.Start,
                dateTo: periodRange.End,
                saveFileDiretory: saveFileDiretory);
            alphalistGenerator.Start();

            return alphalistGenerator.OutputDirectory;
        }
    }
}
