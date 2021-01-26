using System.Data;

namespace AccuPay.Core.Interfaces
{
    public interface IPayrollSummaryExcelFormatReportDataService
    {
        DataTable GetData(int organizationId, int payPeriodFromId, int payPeriodToId, string salaryDistributionType, short isActual, bool keepInOneSheet);
    }
}
