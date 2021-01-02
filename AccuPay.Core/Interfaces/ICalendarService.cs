using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public interface ICalendarService
    {
        Task<CalendarCollection> GetCalendarCollectionAsync(TimePeriod timePeriod);
    }
}