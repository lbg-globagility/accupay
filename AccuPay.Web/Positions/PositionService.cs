using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Threading.Tasks;

namespace AccuPay.Web.Positions
{
    public class PositionService
    {
        private readonly PositionDataService _service;
        private readonly ICurrentUser _currentUser;

        public PositionService(PositionDataService service, ICurrentUser currentUser)
        {
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<PositionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _service.GetPaginatedListAsync(options, _currentUser.OrganizationId, searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<PositionDto> GetById(int id)
        {
            var positions = await _service.GetByIdWithDivisionAsync(id);

            return ConvertToDto(positions);
        }

        public async Task<PositionDto> Create(CreatePositionDto dto)
        {
            var overtime = new Position()
            {
                CreatedBy = _currentUser.DesktopUserId,
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, overtime);

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task<PositionDto> Update(int id, UpdatePositionDto dto)
        {
            var overtime = await _service.GetByIdAsync(id);
            if (overtime == null) return null;

            overtime.LastUpdBy = _currentUser.DesktopUserId;

            ApplyChanges(dto, overtime);

            await _service.SaveAsync(overtime);

            return ConvertToDto(overtime);
        }

        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
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
