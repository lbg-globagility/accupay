using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
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

        public async Task SaveImportAsync(IReadOnlyCollection<TimeLog> timeLogs,
                                        IReadOnlyCollection<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            await _repository.SaveImportAsync(timeLogs, timeAttendanceLogs);
        }
    }
}