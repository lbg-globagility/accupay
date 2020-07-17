using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class TimeLogDataService : BaseDataService
    {
        private readonly TimeLogRepository _timeLogRepository;

        public TimeLogDataService(
            TimeLogRepository timeLogRepository,
            PayPeriodRepository payPeriodRepository,
            PolicyHelper policy) :

            base(payPeriodRepository,
                policy)
        {
            _timeLogRepository = timeLogRepository;
        }

        #region Save

        public async Task SaveImportAsync(
            int organizationId,
            List<TimeLog> timeLogs,
            List<TimeAttendanceLog> timeAttendanceLogs = null)
        {
            if (timeLogs != null)
            {
                timeLogs.ForEach(x => SanitizeEntity(x));
                await CheckIfDataIsWithinClosedPayroll(timeLogs.Select(x => x.LogDate).Distinct(), organizationId);
            }

            await _timeLogRepository.SaveImportAsync(timeLogs, timeAttendanceLogs);
        }

        public async Task ChangeManyAsync(
            int organizationId,
            List<TimeLog> addedTimeLogs = null,
            List<TimeLog> updatedTimeLogs = null,
            List<TimeLog> deletedTimeLogs = null)
        {
            // TODO: add and update time attendance log for every time log

            if (addedTimeLogs == null && updatedTimeLogs == null && deletedTimeLogs == null)
                throw new BusinessLogicException("No logs to be saved.");

            if (addedTimeLogs != null)
            {
                addedTimeLogs.ForEach(x => SanitizeEntity(x));
                await CheckIfDataIsWithinClosedPayroll(addedTimeLogs.Select(x => x.LogDate).Distinct(), organizationId);
            }

            if (updatedTimeLogs != null)
            {
                updatedTimeLogs.ForEach(x => SanitizeEntity(x));
                await CheckIfDataIsWithinClosedPayroll(updatedTimeLogs.Select(x => x.LogDate).Distinct(), organizationId);
            }

            if (deletedTimeLogs != null)
            {
                await CheckIfDataIsWithinClosedPayroll(deletedTimeLogs.Select(x => x.LogDate).Distinct(), organizationId);
            }

            await _timeLogRepository.ChangeManyAsync(
                added: addedTimeLogs,
                updated: updatedTimeLogs,
                deleted: deletedTimeLogs);
        }

        private void SanitizeEntity(TimeLog timeLog)
        {
            if (timeLog == null)
                throw new BusinessLogicException("Invalid data.");

            if (timeLog.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (timeLog.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (timeLog.LogDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (timeLog.TimeIn == null && timeLog.TimeOut == null)
                throw new BusinessLogicException("Time-in and Time-out cannot be both empty.");

            if (timeLog.TimeIn != null && timeLog.TimeStampIn == null)
            {
                timeLog.TimeStampIn = timeLog.TimeInFull;
            }

            if (timeLog.TimeOut != null && timeLog.TimeStampOut == null)
            {
                timeLog.TimeStampOut = timeLog.TimeOutFull;
            }
        }

        #endregion Save
    }
}