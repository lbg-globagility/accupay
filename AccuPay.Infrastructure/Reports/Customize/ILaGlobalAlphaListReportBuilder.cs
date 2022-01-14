using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Reports.Customize
{
    public interface ILaGlobalAlphaListReportBuilder
    {
        Task GenerateReportAsync(int organizationId,
            bool actualSwitch,
            int startPeriodId,
            int endPeriodId,
            string saveFilePath);
    }
}
