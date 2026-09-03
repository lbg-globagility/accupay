using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IAccessOffshoringPayslip
    {
        Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            string saveFilePath);
    }
}
