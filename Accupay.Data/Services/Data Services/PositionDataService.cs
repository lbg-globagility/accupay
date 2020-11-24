using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PositionDataService : BaseSavableDataService<Position>
    {
        private readonly PositionRepository _positionRepository;
        private readonly EmployeeRepository _employeeRepository;
        private readonly DivisionDataService _divisionService;

        public PositionDataService(
            PositionRepository positionRepository,
            EmployeeRepository employeeRepository,
            PayPeriodRepository payPeriodRepository,
            DivisionDataService divisionService,
            PayrollContext context,
            PolicyHelper policy) :

            base(positionRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Position")
        {
            _positionRepository = positionRepository;

            _employeeRepository = employeeRepository;

            _divisionService = divisionService;
        }

        public async override Task DeleteAsync(int positionId)
        {
            var position = await _positionRepository.GetByIdAsync(positionId);

            if (position == null)
                throw new BusinessLogicException("Position does not exists.");

            if ((await _employeeRepository.GetByPositionAsync(positionId)).Any())
                throw new BusinessLogicException("Position already has at least one assigned employee therefore cannot be deleted.");

            await _positionRepository.DeleteAsync(position);
        }

        protected override async Task SanitizeEntity(Position position, Position oldPosition)
        {
            if (position.DivisionID == null)
                throw new BusinessLogicException("Division is required.");

            if (position.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

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
