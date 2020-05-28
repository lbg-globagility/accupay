using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Positions
{
    public class PositionService
    {
        private readonly PositionRepository _repository;

        public PositionService(PositionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<PositionDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<PositionDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
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
