using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogService
    {
        private TimeLogRepository _timeLogRepository;

        public TimeLogService(TimeLogRepository timeLogRepository)
        {
            _timeLogRepository = timeLogRepository;
        }

        internal async Task<IEnumerable<TimeLog>> GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(int[] employeeIds, TimePeriod timePeriod)
        {
            var list = await _timeLogRepository.GetByMultipleEmployeeAndDatePeriodWithEmployeeAsync(employeeIds, timePeriod);

            return list;
        }
    }
}
