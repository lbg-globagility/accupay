using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class PositionDataService : BaseDataService
    {
        private readonly PositionRepository _positionRepository;
        private readonly EmployeeRepository _employeeRepository;

        public PositionDataService(PositionRepository positionRepository, EmployeeRepository employeeRepository)
        {
            _positionRepository = positionRepository;

            _employeeRepository = employeeRepository;
        }

        public async Task DeleteAsync(int positionId)
        {
            var position = await _positionRepository.GetByIdAsync(positionId);

            if (position == null)
                throw new BusinessLogicException("Position does not exists.");

            if ((await _employeeRepository.GetByPositionAsync(positionId)).Any())
                throw new BusinessLogicException("Position already has at least one assigned employee therefore cannot be deleted.");

            await _positionRepository.DeleteAsync(positionId);
        }

        public async Task SaveAsync(Position position)
        {
            if (position.DivisionID == null)
                throw new BusinessLogicException("Division is required.");

            if (position.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            var existingPosition = await _positionRepository.GetByNameAsync(position.OrganizationID.Value, position.Name);

            if (existingPosition != null)
            {
                // insert
                if (isNewEntity(position.RowID))
                {
                    throw new BusinessLogicException("Position name already exists!");
                }
                // update
                else if (position.RowID.Value != existingPosition.RowID.Value)
                {
                    throw new BusinessLogicException("Position name already exists!");
                }
            }

            await _positionRepository.SaveAsync(position);
        }

        public async Task<Position> GetByIdAsync(int positionId)
        {
            return await _positionRepository.GetByIdAsync(positionId);
        }

        public async Task<Position> GetByIdWithDivisionAsync(int positionId)
        {
            return await _positionRepository.GetByIdWithDivisionAsync(positionId);
        }

        public async Task<PaginatedListResult<Position>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm)
        {
            return await _positionRepository.GetPaginatedListAsync(options, organizationId, searchTerm);
        }
    }
}