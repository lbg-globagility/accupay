using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces.Reports
{
    public interface IAlphalistReportBuilder
    {
        Task<string> GenerateReportAsync(
            int organizationId,
            bool actualSwitch,
            PayPeriod startPeriod,
            PayPeriod endPeriod,
            string saveFileDiretory);
    }
}
