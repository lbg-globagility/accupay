using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Reports;
using AccuPay.Core.ValueObjects;
using AccuPay.Infrastructure.Reports.Service;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports
{
    public class AlphalistReportBuilder : IAlphalistReportBuilder
    {
        private readonly IAlphaListReportDataService _alphaListReportDataService;
        private readonly IOrganizationRepository _organizationRepository;

        public AlphalistReportBuilder(IAlphaListReportDataService alphaListReportDataService,
            IOrganizationRepository organizationRepository)
        {
            _alphaListReportDataService = alphaListReportDataService;
            _organizationRepository = organizationRepository;
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

            var properModels = models.
                Where(t => t.StartDate < t.EndDate).
                ToList();

            var organization = await _organizationRepository.GetByIdAsync(organizationId);

            var alphalistGenerator = new AlphalistGenerator(models: properModels,
                year: 0,
                dateFrom: periodRange.Start,
                dateTo: periodRange.End,
                saveFileDiretory: saveFileDiretory,
                organization: organization,
                endPeriod: endPeriod);
            alphalistGenerator.Start();

            return alphalistGenerator.OutputDirectory;
        }
    }
}
