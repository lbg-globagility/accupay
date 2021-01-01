using AccuPay.Core.Entities;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Calendars
{
    public class CalendarService
    {
        private readonly CalendarRepository _repository;

        private readonly DayTypeRepository _dayTypeRepository;

        private readonly CalendarDataService _dataService;

        private readonly ICurrentUser _currentUser;

        public CalendarService(
            CalendarRepository repository,
            DayTypeRepository dayTypeRepository,
            CalendarDataService dataService,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _dayTypeRepository = dayTypeRepository;
            _dataService = dataService;
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
            var calendar = PayCalendar.NewCalendar(dto.Name);
            calendar.CreatedBy = _currentUser.UserId;

            var copiedCalendar = await _repository.GetById(dto.CopiedCalendarId);

            await _dataService.CreateAsync(calendar, copiedCalendar);

            return ConvertToDto(calendar);
        }

        public async Task<CalendarDto> Update(int id, UpdateCalendarDto dto)
        {
            var calendar = await _repository.GetById(id);
            calendar.Name = dto.Name;
            calendar.LastUpdBy = _currentUser.UserId;

            await _dataService.UpdateAsync(calendar);

            return ConvertToDto(calendar);
        }

        public async Task UpdateDays(int calendarId, ICollection<CalendarDayDto> dtos)
        {
            var dates = dtos.Select(t => t.Date.Date).ToList();
            var calendarDays = await _repository.GetCalendarDays(calendarId, dates);
            var dayTypes = await _dayTypeRepository.GetAllAsync();

            var added = new Collection<CalendarDay>();
            var updated = new Collection<CalendarDay>();

            foreach (var dto in dtos)
            {
                var dayType = dayTypes.First(t => t.Name == dto.DayType);

                var calendarDay = calendarDays.FirstOrDefault(t => t.Date == dto.Date.Date);

                if (calendarDay == null)
                {
                    calendarDay = new CalendarDay()
                    {
                        CalendarID = calendarId,
                        Date = dto.Date,
                        CreatedBy = _currentUser.UserId,
                    };

                    added.Add(calendarDay);
                }
                else
                {
                    calendarDay.UpdatedBy = _currentUser.UserId;
                    updated.Add(calendarDay);
                }

                calendarDay.DayTypeID = dayType.RowID;
                calendarDay.Description = dto.Description;
            }

            await _dataService.UpdateDaysAsync(added, updated);
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
