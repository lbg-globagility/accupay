using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;

namespace AccuPay.Infrastructure.Data
{
    public class TimeAttendanceLogDataService : BaseOrganizationDataService<TimeAttendanceLog>, ITimeAttendanceLogDataService
    {
        private const string UserActivityName = "TimeAttendanceLog";

        private readonly ITimeAttendanceLogRepository _timeAttendanceLogRepository;

        public TimeAttendanceLogDataService(ITimeAttendanceLogRepository timeAttendanceLogRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(timeAttendanceLogRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "TimeAttendanceLog")
        {
            _timeAttendanceLogRepository = timeAttendanceLogRepository;
        }

        protected override string CreateUserActivitySuffixIdentifier(TimeAttendanceLog entity)
            => UserActivityName;

        protected override string GetUserActivityName(TimeAttendanceLog entity)
            => UserActivityName;
    }
}
