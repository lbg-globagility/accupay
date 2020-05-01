using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TimeLogRepository
    {
        #region CRUD

        public async Task ChangeManyAsync(List<TimeLog> addedTimeLogs,
                                        List<TimeLog> updatedTimeLogs,
                                        List<TimeLog> deletedTimeLogs)
        {
            using (var context = new PayrollContext())
            {
                // insert
                addedTimeLogs.ForEach(timeLog =>
                {
                    context.Entry(timeLog).State = EntityState.Added;
                });

                // update
                updatedTimeLogs.ForEach(timeLog =>
                {
                    context.Entry(timeLog).State = EntityState.Modified;
                });

                // delete
                deletedTimeLogs = deletedTimeLogs.
                                    GroupBy(x => x.RowID).
                                    Select(x => x.FirstOrDefault()).
                                    ToList();

                //deletedTimeLogs.ForEach(timeLog =>
                //{
                //    context.Entry(timeLog).State = EntityState.Deleted;
                //});

                context.TimeLogs.RemoveRange(deletedTimeLogs);

                await context.SaveChangesAsync();
            }
        }

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

        #endregion CRUD

        #region Queries

        public async Task<IEnumerable<TimeLog>> GetByEmployeeAndDateAsync(int employeeId, DateTime date)
        {
            using (var context = new PayrollContext())
            {
                return await context.TimeLogs.
                            Where(x => x.EmployeeID == employeeId).
                            Where(x => x.LogDate == date).
                            ToListAsync();
            }
        }

        public IEnumerable<TimeLog> GetByDatePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return CreateBaseQueryByDatePeriod(timePeriod, context).
                        Where(x => x.OrganizationID == organizationId).
                        ToList();
            }
        }

        public async Task<IEnumerable<TimeLog>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(
                                                                                    int[] employeeIds,
                                                                                    TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return await CreateBaseQueryByDatePeriod(timePeriod, context).
                            Include(x => x.Employee).
                            Where(x => employeeIds.Contains(x.EmployeeID.Value)).
                            ToListAsync();
            }
        }

        private IQueryable<TimeLog> CreateBaseQueryByDatePeriod(TimePeriod timePeriod, PayrollContext context)
        {
            return context.TimeLogs.
                        Where(x => timePeriod.Start <= x.LogDate).
                        Where(x => x.LogDate <= timePeriod.End);
        }

        #endregion Queries

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