using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface INonBdoBankReportBuilder
    {
        Task CreateReport(int organizationId, TimePeriod timePeriod, string saveFilePath);
    }
}
