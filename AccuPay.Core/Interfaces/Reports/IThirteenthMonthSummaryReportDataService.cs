using AccuPay.Core.ValueObjects;
using System.Data;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IThirteenthMonthSummaryReportDataService
    {
        Task<DataTable> GetData(int organizationId, TimePeriod timePeriod);
    }
}
