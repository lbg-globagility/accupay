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
        public async Task<PayCalendar> GetById(int calendarID)
        {
            using (var context = new PayrollContext())
            {
                return await context.Calendars.
                                    Where(c => c.RowID.Value == calendarID).
                                    FirstOrDefaultAsync();
            }
        }

        public async Task<ICollection<PayCalendar>> GetAllAsync()
        {
            using (var context = new PayrollContext())
            {
                return await context.Calendars.ToListAsync();
            }
        }

        /// <summary>
        ///         ''' Gets all days of a calendar that is part of a given year
        ///         ''' </summary>
        ///         ''' <param name="calendarID"></param>
        ///         ''' <param name="year"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarID, int year)
        {
            var firstDayOfYear = new DateTime(year, 1, 1);
            var lastDayOfYear = new DateTime(year, 12, 31);

            return await GetCalendarDays(calendarID, firstDayOfYear, lastDayOfYear);
        }

        /// <summary>
        ///         ''' Gets all days of a calendar that is from and to the given date range
        ///         ''' </summary>
        ///         ''' <param name="calendarID"></param>
        ///         ''' <param name="from"></param>
        ///         ''' <param name="[to]"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarID,
                                                                    DateTime from,
                                                                    DateTime to)
        {
            using (var context = new PayrollContext())
            {
                return await context.CalendarDays.
                                    Include(t => t.DayType).
                                    Where(t => from <= t.Date && t.Date <= to).
                                    Where(t => t.CalendarID.Value == calendarID).
                                    ToListAsync();
            }
        }

        /// <summary>
        ///         ''' Gets all days of a calendar
        ///         ''' </summary>
        ///         ''' <param name="calendarID"></param>
        ///         ''' <returns></returns>
        public async Task<ICollection<CalendarDay>> GetCalendarDays(int calendarID)
        {
            using (var context = new PayrollContext())
            {
                return await context.CalendarDays.
                                Include(t => t.DayType).
                                Where(t => t.CalendarID.Value == calendarID).
                                ToListAsync();
            }
        }

        public async Task Create(PayCalendar calendar, PayCalendar copiedCalendar)
        {
            using (var context = new PayrollContext())
            {
                var transaction = await context.Database.BeginTransactionAsync();

                try
                {
                    context.Calendars.Add(calendar);
                    await context.SaveChangesAsync();

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

                    context.CalendarDays.AddRange(newDays);
                    await context.SaveChangesAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task UpdateManyAsync(ICollection<CalendarDay> calendarDays)
        {
            using (var context = new PayrollContext())
            {
                foreach (var calendarDay in calendarDays)
                {
                    context.Entry(calendarDay).State = EntityState.Modified;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}