using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PositionDataService : BaseOrganizationDataService<Position>
    {
        private const string UserActivityName = "Position";

        private readonly PositionRepository _positionRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly DivisionDataService _divisionService;
        private readonly DivisionRepository _divisionRepository;

        public PositionDataService(
            PositionRepository positionRepository,
            EmployeeRepository employeeRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            DivisionDataService divisionService,
            PayrollContext context,
            IPolicyHelper policy,
            DivisionRepository divisionRepository) :

            base(positionRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Position")
        {
            _positionRepository = positionRepository;

            _employeeRepository = employeeRepository;

            _divisionService = divisionService;
            _divisionRepository = divisionRepository;
        }

        public async Task<Position> GetByNameOrCreateAsync(string positionName, int organizationId, int currentlyLoggedInUserId)
        {
            var existingPosition = await _positionRepository.GetByNameAsync(organizationId, positionName);

            if (existingPosition != null) return existingPosition;

            var defaultDivision = await _divisionService
                .GetOrCreateDefaultDivisionAsync(
                    organizationId: organizationId,
                    changedByUserId: currentlyLoggedInUserId);

            if (defaultDivision?.RowID == null)
                throw new BusinessLogicException("Cannot create default division.");

            var position = new Position()
            {
                OrganizationID = organizationId,
                Name = positionName,
                DivisionID = defaultDivision.RowID.Value
            };

            await SaveAsync(position, currentlyLoggedInUserId);

            return position;
        }

        protected override string GetUserActivityName(Position position)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(Position position)
        {
            return $" with name '{position.Name}'";
        }

        public override async Task DeleteAsync(int positionId, int currentlyLoggedInUserId)
        {
            var position = await _positionRepository.GetByIdAsync(positionId);

            if (position == null)
                throw new BusinessLogicException("Position does not exists.");

            if ((await _employeeRepository.GetByPositionAsync(positionId)).Any())
                throw new BusinessLogicException("Position already has at least one assigned employee therefore cannot be deleted.");

            await base.DeleteAsync(
                id: positionId,
                currentlyLoggedInUserId: currentlyLoggedInUserId);
        }

        protected override async Task SanitizeEntity(Position position, Position oldPosition, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: position,
                oldEntity: oldPosition,
                currentlyLoggedInUserId: changedByUserId);

            if (position.DivisionID == null)
                throw new BusinessLogicException("Division is required.");

            var existingPosition = await _positionRepository.GetByNameAsync(position.OrganizationID.Value, position.Name);

            if (existingPosition != null)
            {
                // insert
                if (position.IsNewEntity)
                {
                    throw new BusinessLogicException("Position name already exists!");
                }
                // update
                else if (position.RowID.Value != existingPosition.RowID.Value)
                {
                    throw new BusinessLogicException("Position name already exists!");
                }
            }
        }

        protected override async Task PostDeleteAction(Position entity, int currentlyLoggedInUserId)
        {
            // supplying Division data for saving useractivity
            entity.Division = await _divisionRepository.GetByIdAsync(entity.DivisionID.Value);

            await base.PostDeleteAction(entity, currentlyLoggedInUserId);
        }

        protected override async Task PostSaveAction(Position entity, Position oldEntity, SaveType saveType)
        {
            // supplying Division data for saving useractivity
            entity.Division = await _divisionRepository.GetByIdAsync(entity.DivisionID.Value);

            if (oldEntity != null)
            {
                oldEntity.Division = await _divisionRepository.GetByIdAsync(oldEntity.DivisionID.Value);
            }

            await base.PostSaveAction(entity, oldEntity, saveType);
        }

        protected override async Task RecordUpdate(Position newValue, Position oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.DivisionID != oldValue.DivisionID)
            {
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated division from '{oldValue.Division?.Name}' to '{newValue.Division?.Name}' {suffixIdentifier}",
                });
            }
            if (newValue.Name != oldValue.Name)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated name from '{oldValue.Name}' to '{newValue.Name}' {suffixIdentifier}",
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }
    }
}
