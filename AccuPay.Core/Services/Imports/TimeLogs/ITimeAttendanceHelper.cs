using AccuPay.Core.Entities;
using AccuPay.Core.Services.Imports;
using System.Collections.Generic;

namespace AccuPay.Core.Services
{
    public interface ITimeAttendanceHelper
    {
        List<TimeLog> GenerateTimeLogs();

        List<TimeAttendanceLog> GenerateTimeAttendanceLogs();

        List<TimeLogImportModel> Analyze();

        List<TimeLogImportModel> Validate();
    }
}