using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Utilities.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class CalendarDataService : ICalendarDataService
    {
        private readonly ICalendarRepository _calendarRepository;
        private readonly IBranchRepository _branchRepository;

        public CalendarDataService(
            ICalendarRepository calendarRepository,
            IBranchRepository branchRepository)
        {
            _calendarRepository = calendarRepository;
            _branchRepository = branchRepository;
        }

        public async Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar)
        {
            if (copiedCalendar.RowID == null)
                throw new BusinessLogicException("Copied calendar does not exists.");

            if (calendar.Name.ToTrimmedLowerCase() == PayCalendar.DefaultName.ToTrimmedLowerCase())
                throw new BusinessLogicException("Cannot create a calendar with name 'Default'.");

            await _calendarRepository.CreateAsync(calendar, copiedCalendar);
        }

        public async Task UpdateAsync(PayCalendar calendar)
        {
            if (calendar.IsDefault)
            {
                if (calendar.Name.ToTrimmedLowerCase() != PayCalendar.DefaultName.ToTrimmedLowerCase())
                    throw new BusinessLogicException("Cannot change the name of Default calendar.");
            }
            else
            {
                if (calendar.Name.ToTrimmedLowerCase() == PayCalendar.DefaultName.ToTrimmedLowerCase())
                    throw new BusinessLogicException("Cannot change the name of calendar into 'Default'.");
            }

            await _calendarRepository.UpdateAsync(calendar);
        }

        public async Task UpdateDaysAsync(ICollection<CalendarDay> added, ICollection<CalendarDay> updated)
        {
            await _calendarRepository.UpdateDaysAsync(added, updated);
        }

        public async Task DeleteAsync(PayCalendar calendar)
        {
            if (calendar.IsDefault)
                throw new BusinessLogicException("Cannot delete default calendar.");

            if (await _branchRepository.HasCalendarAsync(calendar))
                throw new BusinessLogicException("Calendar is currently in use.");

            await _calendarRepository.DeleteAsync(calendar);
        }
    }
}
