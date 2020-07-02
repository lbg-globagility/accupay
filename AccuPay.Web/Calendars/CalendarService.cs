using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Calendars
{
    public class CalendarService
    {
        private readonly CalendarRepository _repository;

        private readonly DayTypeRepository _dayTypeRepository;

        private readonly ICurrentUser _currentUser;

        public CalendarService(CalendarRepository repository, DayTypeRepository dayTypeRepository, ICurrentUser currentUser)
        {
            _repository = repository;
            _dayTypeRepository = dayTypeRepository;
            _currentUser = currentUser;
        }

        public async Task<CalendarDto> GetById(int? calendarId)
        {
            var calendar = await _repository.GetById(calendarId.Value);

            return ConvertToDto(calendar);
        }

        public async Task<ICollection<CalendarDayDto>> GetDays(int calendarId, int year)
        {
            var calendarDays = await _repository.GetCalendarDays(calendarId, year);
            var dtos = calendarDays.Select(t => ConvertToDto(t)).ToList();

            return dtos;
        }

        public async Task<ICollection<DayTypeDto>> GetDayTypes()
        {
            var dayTypes = await _dayTypeRepository.GetAllAsync();
            var dtos = dayTypes.Select(t => ConvertToDto(t)).ToList();

            return dtos;
        }

        public async Task<CalendarDto> Create(CreateCalendarDto dto)
        {
            var calendar = new PayCalendar();
            calendar.Name = dto.Name;
            calendar.CreatedBy = _currentUser.DesktopUserId;

            var copiedCalendar = await _repository.GetById(dto.CopiedCalendarId);

            await _repository.CreateAsync(calendar, copiedCalendar);

            return ConvertToDto(calendar);
        }

        public async Task<CalendarDto> Update(int id, UpdateCalendarDto dto)
        {
            var calendar = await _repository.GetById(id);
            calendar.Name = dto.Name;
            calendar.LastUpdBy = _currentUser.DesktopUserId;

            await _repository.Update(calendar);

            return ConvertToDto(calendar);
        }

        public async Task UpdateDays(int calendarId, ICollection<CalendarDayDto> dtos)
        {
            var calendarDayIds = dtos.Select(t => t.Id).ToList();
            var calendarDays = await _repository.GetCalendarDays(calendarDayIds);
            var dayTypes = await _dayTypeRepository.GetAllAsync();

            foreach (var calendarDay in calendarDays)
            {
                var dto = dtos.First(t => t.Id == calendarDay.RowID);
                var dayType = dayTypes.First(t => t.Name == dto.DayType);

                calendarDay.Description = dto.Description;
                calendarDay.DayType = dayType;
                calendarDay.UpdatedBy = _currentUser.DesktopUserId;
            }

            await _repository.UpdateDaysAsync(calendarDays);
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

        private CalendarDayDto ConvertToDto(CalendarDay calendarDay)
        {
            var dto = new CalendarDayDto()
            {
                Id = calendarDay.RowID,
                Date = calendarDay.Date,
                Description = calendarDay.Description,
                DayType = calendarDay.DayType.Name
            };

            return dto;
        }

        private DayTypeDto ConvertToDto(DayType calendarDay)
        {
            var dto = new DayTypeDto()
            {
                Id = calendarDay.RowID,
                Name = calendarDay.Name,
            };

            return dto;
        }
    }
}
