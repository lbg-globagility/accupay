using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDailyAttendanceReport
    {
        Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            string saveFilePath);
    }
}
