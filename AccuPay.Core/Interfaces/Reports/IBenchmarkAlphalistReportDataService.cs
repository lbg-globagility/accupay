using System.Data;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IBenchmarkAlphalistReportDataService
    {
        Task<DataTable> GetData(int organizationId, int year);
    }
}
