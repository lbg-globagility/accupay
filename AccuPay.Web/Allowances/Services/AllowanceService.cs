using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Allowances.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Allowances.Services
{
    public class AllowanceService
    {
        private readonly AllowanceRepository _repository;

        public AllowanceService(AllowanceRepository allowanceRepository)
        {
            _repository = allowanceRepository;
        }

        public async Task<PaginatedList<AllowanceDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<AllowanceDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<AllowanceDto> GetById(int id)
        {
            var allowance = await _repository.GetByIdAsync(id);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Create(CreateAllowanceDto dto)
        {
            // TODO: validations
            var allowance = new Allowance
            {
                OrganizationID = 5,
                CreatedBy = 1,
                EmployeeID = dto.EmployeeId
            };
            ApplyChanges(dto, allowance);

            await _repository.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Update(int id, UpdateAllowanceDto dto)
        {
            // TODO: validations
            var allowance = await _repository.GetByIdAsync(id);
            if (allowance == null) return null;

            allowance.LastUpdBy = 1;

            ApplyChanges(dto, allowance);

            await _repository.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        private static void ApplyChanges(ICrudAllowanceDto dto, Allowance allowance)
        {
            allowance.ProductID = dto.AllowanceTypeId;
            allowance.AllowanceFrequency = dto.Frequency;
            allowance.EffectiveStartDate = dto.StartDate;
            allowance.EffectiveEndDate = dto.EndDate;
            allowance.Amount = dto.Amount;
        }

        private static AllowanceDto ConvertToDto(Allowance allowance)
        {
            if (allowance == null) return null;

            return new AllowanceDto()
            {
                Id = allowance.RowID.Value,
                EmployeeNumber = allowance.Employee?.EmployeeNo,
                EmployeeName = allowance.Employee?.FullNameWithMiddleInitialLastNameFirst,
                AllowanceType = allowance.Type,
                StartDate = allowance.EffectiveStartDate,
                Frequency = allowance.AllowanceFrequency,
                EndDate = allowance.EffectiveEndDate,
                Amount = allowance.Amount
            };
        }
    }
}
