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

        public async Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar)
        {
            if (copiedCalendar.RowID == null)
                throw new Exception("Copied calendar does not exists");

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

        public async Task Update(PayCalendar calendar)
        {
            _context.Entry(calendar).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDaysAsync(ICollection<CalendarDay> calendarDays)
        {
            foreach (var calendarDay in calendarDays)
            {
                _context.Entry(calendarDay).State = EntityState.Modified;
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(PayCalendar payCalendar)
        {
            var isInUse = await _context.Branches
                .Where(b => b.CalendarID == payCalendar.RowID)
                .AnyAsync();

            if (isInUse)
            {
                throw new Exception("Calendar is currently in use");
            }
            else
            {
                var calendarDays = await _context.CalendarDays
                    .Where(d => d.CalendarID == payCalendar.RowID)
                    .ToListAsync();

                _context.Calendars.Remove(payCalendar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<PayCalendar>> GetAllAsync()
        {
            return await _context.Calendars.ToListAsync();
        }

        public async Task<PayCalendar> GetById(int calendarId)
        {
            return await _context.Calendars.FindAsync(calendarId);
        }

        /// <summary>
        ///         ''' Gets all days of a calendar that is part of a given year
        ///         ''' </summary>
        ///         ''' <param name="calendarId"></param>
        ///         ''' <param name="year"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            return await GetCalendarDays(calendarId, firstDayOfYear, lastDayOfYear);
        }

        /// <summary>
        ///         ''' Gets all days of a calendar that is from and to the given date range
        ///         ''' </summary>
        ///         ''' <param name="calendarId"></param>
        ///         ''' <param name="from"></param>
        ///         ''' <param name="[to]"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId,
                                                                    DateTime from,
                                                                    DateTime to)
        {
            return await _context.CalendarDays.
                                Include(t => t.DayType).
                                Where(t => from <= t.Date && t.Date <= to).
                                Where(t => t.CalendarID == calendarId).
                                ToListAsync();
        }

        /// <summary>
        ///         ''' Gets all days of a calendar
        ///         ''' </summary>
        ///         ''' <param name="calendarId"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId)
        {
            return await _context.CalendarDays.
                            Include(t => t.DayType).
                            Where(t => t.CalendarID == calendarId).
                            ToListAsync();
        }
    }
}
