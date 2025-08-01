using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly ICalendarRepository _calendarRepository;
        private readonly IDayTypeRepository _dayTypeRepository;

        public CalendarService(
            IBranchRepository branchRepository,
            ICalendarRepository calendarRepository,
            IDayTypeRepository dayTypeRepository)
        {
            _branchRepository = branchRepository;
            _calendarRepository = calendarRepository;
            _dayTypeRepository = dayTypeRepository;
        }

        public async Task<CalendarCollection> GetCalendarCollectionAsync(TimePeriod timePeriod)
        {
            var branches = await _branchRepository.GetAllAsync();

            var calendarDays = await _calendarRepository.GetCalendarDays(timePeriod.Start, timePeriod.End);

            var dayType = await _dayTypeRepository.GetOrCreateRegularDayAsync();

            var defaultRates = new DefaultRates()
            {
                RegularRate = dayType.RegularRate,
                OvertimeRate = dayType.OvertimeRate,
                NightDiffRate = dayType.NightDiffRate,
                NightDiffOTRate = dayType.NightDiffOTRate,
                RestDayRate = dayType.RestDayRate,
                RestDayOTRate = dayType.RestDayOTRate,
                RestDayNDRate = dayType.RestDayNDRate,
                RestDayNDOTRate = dayType.RestDayNDOTRate,
            };

            var defaultCalendar = await _calendarRepository.GetOrCreateDefaultCalendar();

            return new CalendarCollection(branches, calendarDays, defaultRates, defaultCalendar);
        }
    }
}
