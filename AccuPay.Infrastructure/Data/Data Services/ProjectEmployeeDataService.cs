using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ProjectEmployeeDataService : BaseOrganizationDataService<ProjectEmployee>, IProjectEmployeeDataService
    {
        public const string UserActivityName = nameof(ProjectEmployee);
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;

        public ProjectEmployeeDataService(IProjectEmployeeRepository projectEmployeeRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            string entityName = nameof(ProjectEmployee)) : base(projectEmployeeRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName)
        {
            _projectEmployeeRepository = projectEmployeeRepository;
        }

        public async Task<ICollection<ProjectEmployee>> GetAllByProjectIdAsync(int projectId)
            => await _projectEmployeeRepository.GetAllByProjectIdAsync(projectId);

        protected override string CreateUserActivitySuffixIdentifier(ProjectEmployee entity)
            => UserActivityName;

        protected override string GetUserActivityName(ProjectEmployee entity)
            => UserActivityName;

        protected override async Task RecordUpdate(ProjectEmployee entity, ProjectEmployee oldEntity)
        {
            var diffString = oldEntity.GetPropertyChanges(entity);

            if (string.IsNullOrEmpty(diffString)) return;

            var userActivityItem = new List<UserActivityItem>()
                {
                    new UserActivityItem()
                    {
                        EntityId = oldEntity.RowID.Value,
                        Description = diffString,
                    }
                };

            await _userActivityRepository.CreateRecordAsync(
                oldEntity.LastUpdBy.Value,
                UserActivityName,
                oldEntity.OrganizationID.Value,
                UserActivity.RecordTypeEdit,
                userActivityItem);
        }
    }
}
