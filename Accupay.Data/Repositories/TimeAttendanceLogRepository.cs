using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TimeAttendanceLogRepository
    {
        public IEnumerable<TimeAttendanceLog> GetByTimePeriod(int organizationId, TimePeriod timePeriod)
        {
            using (var context = new PayrollContext())
            {
                return context.TimeAttendanceLogs.
                            Where(x => x.OrganizationID == organizationId).
                            Where(x => timePeriod.Start <= x.WorkDay).
                            Where(x => x.WorkDay <= timePeriod.End).
                            ToList();
            }
        }

        public async Task<IEnumerable<TimeAttendanceLog>> GetByDateAndEmployeeAsync(DateTime date, int employeeId)
        {
            using (var context = new PayrollContext())
            {
                return await context.TimeAttendanceLogs.
                            Where(x => x.EmployeeID == employeeId).
                            Where(x => x.WorkDay == date).
                            ToListAsync();
            }
        }
    }
}