using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class TimeAttendanceLogRepository : ITimeAttendanceLogRepository
    {
        private readonly PayrollContext _context;

        public TimeAttendanceLogRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<TimeAttendanceLog>> GetByTimePeriodAsync(int organizationId, TimePeriod timePeriod)
        {
            return await _context.TimeAttendanceLogs
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => timePeriod.Start <= x.WorkDay)
                .Where(x => x.WorkDay <= timePeriod.End)
                .ToListAsync();
        }

        public async Task<ICollection<TimeAttendanceLog>> GetByDateAndEmployeeAsync(DateTime date, int employeeId)
        {
            return await _context.TimeAttendanceLogs
                .Where(x => x.EmployeeID == employeeId)
                .Where(x => x.WorkDay == date)
                .ToListAsync();
        }
    }
}
