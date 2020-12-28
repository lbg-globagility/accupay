using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
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

        public PositionDataService(
            PositionRepository positionRepository,
            EmployeeRepository employeeRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            DivisionDataService divisionService,
            PayrollContext context,
            IPolicyHelper policy) :

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
        }

        protected override string GetUserActivityName(Position position) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(Position position) => string.Empty;

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
    }
}
