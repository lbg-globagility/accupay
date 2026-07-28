using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ContractorDataService : BaseOrganizationDataService<Contractor>, IContractorDataService
    {
        public const string UserActivityName = nameof(Contractor);

        public ContractorDataService(IContractorRepository contractorRepository,
            IPayPeriodRepository payPeriodRepository,
            IUserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy,
            string entityName = nameof(Contractor)) : base(contractorRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName)
        {
        }

        public async Task<ICollection<Contractor>> GetAllAsync()
            => await _repository.GetAllAsync();

        protected override string CreateUserActivitySuffixIdentifier(Contractor entity)
            => UserActivityName;

        protected override string GetUserActivityName(Contractor entity)
            => UserActivityName;

        protected override async Task RecordUpdate(Contractor entity, Contractor oldEntity)
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
