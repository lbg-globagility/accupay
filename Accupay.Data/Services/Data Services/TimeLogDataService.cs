using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeLogDataService
    {
        private readonly TimeLogRepository _repository;

        public TimeLogDataService(TimeLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedListResult<TimeLog>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm)
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);
        }

        public async Task<(ICollection<Employee> employees, int total, ICollection<TimeLog>)> ListByEmployee(
            int organizationId,
            PageOptions options,
            DateTime dateFrom,
            DateTime dateTo,
            string searchTerm)
        {
            return await _repository.ListByEmployee(organizationId, options, dateFrom, dateTo, searchTerm);
        }

        public async Task<ICollection<TimeLog>> GetAll(
            ICollection<int> employeeIds,
            DateTime dateFrom,
            DateTime dateTo)
        {
            return await _repository.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIds, new TimePeriod(dateFrom, dateTo));
        }

        public async Task SaveImportAsync(IReadOnlyCollection<TimeLog> timeLogs,
                                        IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            await _repository.SaveImportAsync(timeLogs, timeAttendanceLogs);
        }

        public async Task<TimeLog> GetByIdWithEmployeeAsync(int id)
        {
            return await _repository.GetByIdWithEmployeeAsync(id);
        }

        public async Task<TimeLog> GetByIdAsync(int id)
        {
            return await _repository.GetByIdWithEmployeeAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var timeLog = await _repository.GetByIdAsync(id);

            if (timeLog == null)
                throw new BusinessLogicException("Time-log does not exists.");

            await _repository.DeleteAsync(timeLog);
        }

        public async Task UpdateAsync(TimeLog timeLog)
        {
            if (timeLog.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            await _repository.UpdateAsync(timeLog);
        }

        public async Task CreateAsync(TimeLog timeLog)
        {
            if (timeLog.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            var key = new TimeLogRepository.CompositeKey(timeLog.EmployeeID.Value, timeLog.LogDate);
            var existingShift = await _repository.GetByIdAsync(key);

            if (existingShift != null)
                throw new BusinessLogicException("Employee already has a shift for that day.");

            await _repository.CreateAsync(timeLog);
        }
    }
}