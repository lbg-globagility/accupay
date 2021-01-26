using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICalendarRepository
    {
        Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar);

        Task DeleteAsync(PayCalendar payCalendar);

        Task<ICollection<PayCalendar>> GetAllAsync();

        Task<PayCalendar> GetById(int calendarId);

        Task<ICollection<CalendarDay>> GetCalendarDays(DateTime from, DateTime to);

        Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId);

        Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, DateTime from, DateTime to);

        Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, ICollection<DateTime> dates);

        Task<ICollection<CalendarDay>> GetCalendarDays(int calendarId, int year);

        Task<ICollection<CalendarDay>> GetHolidays(int calendarId, DateTime from, DateTime to);

        Task<PayCalendar> GetOrCreateDefaultCalendar();

        Task UpdateAsync(PayCalendar calendar);

        Task UpdateDaysAsync(ICollection<CalendarDay> added, ICollection<CalendarDay> updated);
    }
}
