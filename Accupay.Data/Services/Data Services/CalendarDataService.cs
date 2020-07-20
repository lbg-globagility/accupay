using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class CalendarDataService
    {
        private readonly CalendarRepository _calendarRepository;
        private readonly BranchRepository _branchRepository;

        public CalendarDataService(CalendarRepository calendarRepository, BranchRepository branchRepository)
        {
            _calendarRepository = calendarRepository;
            _branchRepository = branchRepository;
        }

        public async Task CreateAsync(PayCalendar calendar, PayCalendar copiedCalendar)
        {
            if (copiedCalendar.RowID == null)
                throw new BusinessLogicException("Copied calendar does not exists");

            await _calendarRepository.CreateAsync(calendar, copiedCalendar);
        }

        public async Task UpdateAsync(PayCalendar calendar)
        {
            await _calendarRepository.UpdateAsync(calendar);
        }

        public async Task UpdateDaysAsync(ICollection<CalendarDay> added, ICollection<CalendarDay> updated)
        {
            await _calendarRepository.UpdateDaysAsync(added, updated);
        }

        public async Task DeleteAsync(PayCalendar payCalendar)
        {
            if (await _branchRepository.HasCalendarAsync(payCalendar))
                throw new Exception("Calendar is currently in use.");

            await _calendarRepository.DeleteAsync(payCalendar);
        }
    }
}