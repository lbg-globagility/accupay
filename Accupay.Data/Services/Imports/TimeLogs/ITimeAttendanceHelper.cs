using AccuPay.Data.Entities;
using AccuPay.Data.Services.Imports;
using System.Collections.Generic;

namespace AccuPay.Data.Services
{
    public interface ITimeAttendanceHelper
    {
        List<TimeLog> GenerateTimeLogs();

        List<TimeAttendanceLog> GenerateTimeAttendanceLogs();

        List<TimeLogImportModel> Analyze();

        List<TimeLogImportModel> Validate();
    }
}