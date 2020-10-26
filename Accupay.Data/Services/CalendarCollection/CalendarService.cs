using AccuPay.Data.Enums;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class CalendarService
    {
        private readonly PayrollContext _context;
        private readonly BranchRepository _branchRepository;

        public CalendarService(PayrollContext context, BranchRepository branchRepository)
        {
            _context = context;
            _branchRepository = branchRepository;
        }

        public CalendarCollection GetCalendarCollection(TimePeriod timePeriod)
        {
            var defaultCalendar = _context.Calendars.FirstOrDefault(t => t.IsDefault);

            // TODO: create if not existing the DayType with name "Regular Day"
            var dayType = _context.DayTypes.FirstOrDefault(t => t.Name == "Regular Day");

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

            var branches = _branchRepository.GetAll();

            var calendarDays = _context.CalendarDays.
                Include(t => t.DayType).
                Where(t => timePeriod.Start <= t.Date).
                Where(t => t.Date <= timePeriod.End).
                ToList();

            return new CalendarCollection(branches, calendarDays, defaultRates, defaultCalendar);
        }
    }
}