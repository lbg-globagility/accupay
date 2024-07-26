using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICertificationOfDTR
    {
        Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            bool isActual,
            string saveFilePath,
            bool singleSheet);
    }
}
