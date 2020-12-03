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
            PolicyHelper policy) :

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

        protected override string GetUserActivityName(Position salary) => UserActivityName;

        protected override string CreateUserActivitySuffixIdentifier(Position salary) => string.Empty;

        public override async Task DeleteAsync(int positionId, int changedByUserId)
        {
            var position = await _positionRepository.GetByIdAsync(positionId);

            if (position == null)
                throw new BusinessLogicException("Position does not exists.");

            if ((await _employeeRepository.GetByPositionAsync(positionId)).Any())
                throw new BusinessLogicException("Position already has at least one assigned employee therefore cannot be deleted.");

            await base.DeleteAsync(
                id: positionId,
                changedByUserId: changedByUserId);
        }

        protected override async Task SanitizeEntity(Position position, Position oldPosition)
        {
            await base.SanitizeEntity(entity: position, oldEntity: oldPosition);

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

        public async Task<Position> GetByNameOrCreateAsync(string positionName, int organizationId, int userId)
        {
            var existingPosition = await _positionRepository.GetByNameAsync(organizationId, positionName);

            if (existingPosition != null) return existingPosition;

            var defaultDivision = await _divisionService
                .GetOrCreateDefaultDivisionAsync(
                    organizationId: organizationId,
                    userId: userId);

            if (defaultDivision?.RowID == null)
                throw new BusinessLogicException("Cannot create default division.");

            var position = new Position()
            {
                CreatedBy = userId,
                OrganizationID = organizationId,
                Name = positionName,
                DivisionID = defaultDivision.RowID.Value
            };

            await SaveAsync(position);

            return position;
        }
    }
}
