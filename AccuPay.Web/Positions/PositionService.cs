using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Positions
{
    public class PositionService
    {
        private readonly PositionDataService _dataService;

        public PositionService(PositionDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<PaginatedList<PositionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _dataService.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<PositionDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<PositionDto> GetById(int id)
        {
            var positions = await _dataService.GetByIdWithDivisionAsync(id);

            return ConvertToDto(positions);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(id);
        }

        private static PositionDto ConvertToDto(Position overtime)
        {
            if (overtime == null) return null;

            return new PositionDto()
            {
                Id = overtime.RowID.Value,
                Name = overtime.Name,
                DivisionId = overtime.DivisionID.Value,
                DivisionName = overtime.Division?.Name
            };
        }
    }
}
