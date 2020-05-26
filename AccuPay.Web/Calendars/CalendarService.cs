using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Calendars
{
    public class CalendarService
    {
        private readonly CalendarRepository _repository;

        public CalendarService(CalendarRepository repository)
        {
            _repository = repository;
        }

        public async Task<CalendarDto> GetById(int? calendarId)
        {
            var calendar = await _repository.GetById(calendarId.Value);

            return ConvertToDto(calendar);
        }

        public async Task<CalendarDto> Create(CreateCalendarDto dto)
        {
            var calendar = new PayCalendar();
            calendar.Name = dto.Name;

            var copiedCalendar = await _repository.GetById(dto.CopiedCalendarId);

            await _repository.CreateAsync(calendar, copiedCalendar);

            return ConvertToDto(calendar);
        }

        public async Task<CalendarDto> Update(int id, UpdateCalendarDto dto)
        {
            var calendar = await _repository.GetById(id);

            await _repository.Update(calendar);

            return ConvertToDto(calendar);
        }

        public async Task<ICollection<CalendarDto>> List()
        {
            var calendars = await _repository.GetAllAsync();

            return calendars.Select(c => ConvertToDto(c)).ToList();
        }

        private CalendarDto ConvertToDto(PayCalendar calendar)
        {
            var dto = new CalendarDto()
            {
                Id = calendar.RowID,
                Name = calendar.Name
            };

            return dto;
        }
    }
}
