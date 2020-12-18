using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class CalendarRepository
    {
        private readonly PayrollContext _context;

        public CalendarRepository(PayrollContext context)
        {
            _context = context;
        }

        #region Save

        public async Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar)
        {
            var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Calendars.Add(calendar);
                await _context.SaveChangesAsync();

                var copiedDays = await GetCalendarDays(copiedCalendar.RowID.Value);

                var newDays = new Collection<CalendarDay>();
                foreach (var copiedDay in copiedDays)
                {
                    var day = new CalendarDay();
                    day.CalendarID = calendar.RowID;
                    day.Date = copiedDay.Date;
                    day.DayTypeID = copiedDay.DayTypeID;
                    day.Description = copiedDay.Description;

                    newDays.Add(day);
                }

                _context.CalendarDays.AddRange(newDays);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task UpdateAsync(PayCalendar calendar)
        {
            _context.Entry(calendar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDaysAsync(ICollection<CalendarDay> added, ICollection<CalendarDay> updated)
        {
            foreach (var calendarDay in added)
            {
                _context.CalendarDays.Add(calendarDay);
                // Detach `DayType` from calendar day so it doesn't get reinserted accidentally
                _context.Entry(calendarDay.DayType).State = EntityState.Detached;
            }

            foreach (var calendarDay in updated)
            {
                _context.Entry(calendarDay).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(PayCalendar payCalendar)
        {
            var calendarDays = await _context.CalendarDays
                .Where(d => d.CalendarID == payCalendar.RowID)
                .ToListAsync();

            _context.Calendars.Remove(payCalendar);
            await _context.SaveChangesAsync();
        }

        #endregion Save

        #region Queries

        public async Task<ICollection<PayCalendar>> GetAllAsync()
        {
            return await _context.Calendars.ToListAsync();
        }

        public async Task<PayCalendar> GetById(int calendarId)
        {
            return await _context.Calendars.FindAsync(calendarId);
        }

        public async Task<PayCalendar> GetOrCreateDefaultCalendar()
        {
            var defaultCalendar = await _context.Calendars.FirstOrDefaultAsync(t => t.IsDefault);

            if (defaultCalendar == null)
            {
                defaultCalendar = PayCalendar.CreateDefaultCalendar();

                _context.Calendars.Add(defaultCalendar);
                await _context.SaveChangesAsync();
            }

            return defaultCalendar;
        }

        #region CalendarDay

        /// <summary>
        /// Gets all days of a calendar that is part of a given year
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            return await GetCalendarDays(calendarId, firstDayOfYear, lastDayOfYear);
        }

        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, ICollection<DateTime> dates)
        {
            var calendarDays = await _context.CalendarDays
                .Where(t => t.CalendarID == calendarId)
                .Where(t => dates.Contains(t.Date))
                .ToListAsync();

            return calendarDays;
        }

        /// <summary>
        /// Gets all days of a calendar that is from and to the given date range
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="from"></param>
        /// <param name="[to]"></param>
        /// <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(
            int calendarId,
            DateTime from,
            DateTime to)
        {
            return await _context.CalendarDays
                .Include(t => t.DayType)
                .Where(t => from <= t.Date && t.Date <= to)
                .Where(t => t.CalendarID == calendarId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all days of ALL calendar that is from and to the given date range
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="from"></param>
        /// <param name="[to]"></param>
        /// <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(
            DateTime from,
            DateTime to)
        {
            return await _context.CalendarDays
                .Include(t => t.DayType)
                .Where(t => from <= t.Date && t.Date <= to)
                .ToListAsync();
        }

        /// <summary>
        /// Gets holidays of a calendar that is from and to the given date range
        /// </summary>
        /// <param name="calendarId"></param>
        /// <param name="from"></param>
        /// <param name="[to]"></param>
        /// <returns></returns>
        public async Task<ICollection<CalendarDay>> GetHolidays(
            int calendarId,
            DateTime from,
            DateTime to)
        {
            return await _context.CalendarDays
                .Include(t => t.DayType)
                .Where(t => !t.IsRegularDay)
                .Where(t => from <= t.Date && t.Date <= to)
                .Where(t => t.CalendarID == calendarId)
                .ToListAsync();
        }

        /// <summary>
        /// Gets all days of a calendar
        /// </summary>
        /// <param name="calendarId"></param>
        /// <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId)
        {
            return await _context.CalendarDays
                .Include(t => t.DayType)
                .Where(t => t.CalendarID == calendarId)
                .ToListAsync();
        }

        #endregion CalendarDay

        #endregion Queries
    }
}
