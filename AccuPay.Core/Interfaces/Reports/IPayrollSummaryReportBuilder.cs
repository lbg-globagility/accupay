using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayrollSummaryReportBuilder
    {
        Task CreateReport(
            bool keepInOneSheet,
            bool hideEmptyColumns,
            int organizationId,
            int payPeriodFromId,
            int payPeriodToId,
            string salaryDistributionType,
            bool isActual,
            string saveFilePath);
    }
}