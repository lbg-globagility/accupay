using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface ITaxMonthlyReportDataService
    {
        DataTable GetData(int organizationId, int month, int year);
    }
}
