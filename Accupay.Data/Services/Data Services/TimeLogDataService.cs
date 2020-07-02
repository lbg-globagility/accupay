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

        public async Task SaveImportAsync(IReadOnlyCollection<TimeLog> timeLogs,
                                        IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            await _repository.SaveImportAsync(timeLogs, timeAttendanceLogs);
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

        public async Task ChangeManyAsync(
            List<TimeLog> addedTimeLogs = null,
            List<TimeLog> updatedTimeLogs = null,
            List<TimeLog> deletedTimeLogs = null)
        {
            if (addedTimeLogs == null && updatedTimeLogs == null && deletedTimeLogs == null)
                throw new BusinessLogicException("No logs to be saved.");

            // TODO: validations
            await _repository.ChangeManyAsync(
                addedTimeLogs: addedTimeLogs,
                updatedTimeLogs: updatedTimeLogs,
                deletedTimeLogs: deletedTimeLogs);
        }
    }
}