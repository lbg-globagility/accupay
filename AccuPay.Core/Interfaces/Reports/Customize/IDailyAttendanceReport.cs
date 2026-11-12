using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IDailyAttendanceReport
    {
        /// <summary>
        /// Creates the daily attendance report. If <paramref name="saveFilePath"/> already exists,
        /// the sheets are appended to that workbook.
        /// </summary>
        /// <param name="sheetNameSuffix">Optional text appended to each employee sheet name (e.g. " - Timesheet").</param>
        Task CreateReport(
            int organizationId,
            int payPeriodId,
            int[] employeeIds,
            string saveFilePath,
            string sheetNameSuffix = null);
    }
}
