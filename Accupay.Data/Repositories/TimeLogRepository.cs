using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TimeLogRepository
    {
        public async Task SaveImportAsync(List<TimeLog> timeLogs, List<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            using (var context = new PayrollContext())
            {
                string importId = GenerateImportId(context);

                foreach (var timeLog in timeLogs)
                {
                    timeLog.TimeentrylogsImportID = importId;

                    context.TimeLogs.Add(timeLog);

                    var minimumDate = timeLog.LogDate.ToMinimumHourValue();
                    var maximumDate = timeLog.LogDate.ToMaximumHourValue();

                    context.TimeAttendanceLogs.
                                RemoveRange(context.TimeAttendanceLogs.
                                                        Where(t => t.TimeStamp >= minimumDate).
                                                        Where(t => t.TimeStamp <= maximumDate));
                }

                if (timeAttendanceLogs != null)
                {
                    foreach (var timeAttendanceLog in timeAttendanceLogs)
                    {
                        timeAttendanceLog.ImportNumber = importId;

                        context.TimeAttendanceLogs.Add(timeAttendanceLog);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private string GenerateImportId(PayrollContext context)
        {
            var importId = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var originalImportId = importId;

            int counter = 0;

            while (context.TimeLogs.
                        FirstOrDefault(t => t.TimeentrylogsImportID == importId) != null ||
                                        context.TimeAttendanceLogs.
                                            FirstOrDefault(t => t.ImportNumber == importId) != null)
            {
                counter += 1;

                importId = originalImportId + "_" + counter;
            }

            return importId;
        }
    }
}