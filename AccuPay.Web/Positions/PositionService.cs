using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Threading.Tasks;

namespace AccuPay.Web.Positions
{
    public class PositionService
    {
        private readonly PositionDataService _dataService;
        private readonly PositionRepository _repository;
        private readonly ICurrentUser _currentUser;

        public PositionService(PositionDataService dataService, PositionRepository repository, ICurrentUser currentUser)
        {
            _dataService = dataService;
            _repository = repository;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<PositionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _repository.GetPaginatedListAsync(options, _currentUser.OrganizationId, searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<PositionDto> GetById(int id)
        {
            var positions = await _repository.GetByIdWithDivisionAsync(id);

            return ConvertToDto(positions);
        }

        public async Task<PositionDto> Create(CreatePositionDto dto)
        {
            var overtime = new Position()
            {
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, overtime);

            await _dataService.SaveAsync(overtime, _currentUser.UserId);

            return ConvertToDto(overtime);
        }

        public async Task<PositionDto> Update(int id, UpdatePositionDto dto)
        {
            var overtime = await _repository.GetByIdAsync(id);
            if (overtime == null) return null;

            ApplyChanges(dto, overtime);

            await _dataService.SaveAsync(overtime, _currentUser.UserId);

            return ConvertToDto(overtime);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                positionId: id,
                currentlyLoggedInUserId: _currentUser.UserId);
        }

        private static void ApplyChanges(CrudPositionDto dto, Position position)
        {
            position.DivisionID = dto.DivisionId;
            position.Name = dto.Name;
        }

        private static PositionDto ConvertToDto(Position position)
        {
            if (position == null) return null;

            return new PositionDto()
            {
                Id = position.RowID.Value,
                Name = position.Name,
                DivisionId = position.DivisionID,
                DivisionName = position.Division?.Name
            };
        }
    }
}
