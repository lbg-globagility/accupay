using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ContractorProjectDataService : BaseOrganizationDataService<ContractorProject>, IContractorProjectDataService
    {
        public const string UserActivityName = nameof(ContractorProject);

        public ContractorProjectDataService(IContractorProjectRepository contractorProjectRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            string entityName = nameof(ContractorProject)) : base(contractorProjectRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName)
        {
        }

        public async Task<ICollection<ContractorProject>> GetAllAsync()
            => await _repository.GetAllAsync();

        protected override string CreateUserActivitySuffixIdentifier(ContractorProject entity)
            => UserActivityName;

        protected override string GetUserActivityName(ContractorProject entity)
            => UserActivityName;

        protected override async Task RecordUpdate(ContractorProject entity, ContractorProject oldEntity)
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
