using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICalendarDataService
    {
        Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar);

        Task DeleteAsync(PayCalendar calendar);

        Task UpdateAsync(PayCalendar calendar);

        Task UpdateDaysAsync(ICollection<CalendarDay> added, ICollection<CalendarDay> updated);
    }
}
