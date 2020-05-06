using AccuPay.Data.Entities;
using System.Collections.Generic;

namespace AccuPay.Data.Services
{
    public interface ITimeAttendanceHelper
    {
        List<TimeLog> GenerateTimeLogs();

        List<TimeAttendanceLog> GenerateTimeAttendanceLogs();

        List<ImportTimeAttendanceLog> Analyze();

        List<ImportTimeAttendanceLog> Validate();
    }
}