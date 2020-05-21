using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Products;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Allowances.Services
{
    public class AllowanceService
    {
        private readonly AllowanceRepository _allowanceRepository;
        private readonly ProductRepository _productRepository;

        public AllowanceService(AllowanceRepository allowanceRepository, ProductRepository productRepository)
        {
            _allowanceRepository = allowanceRepository;
            _productRepository = productRepository;
        }

        public async Task<PaginatedList<AllowanceDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _allowanceRepository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<AllowanceDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<AllowanceDto> GetById(int id)
        {
            var allowance = await _allowanceRepository.GetByIdWithEmployeeAndProductAsync(id);

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

            await _allowanceRepository.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Update(int id, UpdateAllowanceDto dto)
        {
            // TODO: validations
            var allowance = await _allowanceRepository.GetByIdAsync(id);
            if (allowance == null) return null;

            allowance.LastUpdBy = 1;

            ApplyChanges(dto, allowance);

            await _allowanceRepository.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        public async Task<List<ProductDto>> GetAllowanceTypes()
        {
            int organizationId = 2;
            var leaveTypes = await _productRepository.GetAllowanceTypesAsync(organizationId);

            return leaveTypes
                    .Where(x => !string.IsNullOrWhiteSpace(x.PartNo))
                    .Select(x => ProductDto.FromProduct(x))
                    .ToList();
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
                EmployeeId = allowance.EmployeeID.Value,
                EmployeeNumber = allowance.Employee?.EmployeeNo,
                EmployeeName = allowance.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = allowance.Employee?.EmployeeType,
                AllowanceTypeId = allowance.ProductID.Value,
                AllowanceType = allowance.Type,
                StartDate = allowance.EffectiveStartDate,
                Frequency = allowance.AllowanceFrequency,
                EndDate = allowance.EffectiveEndDate,
                Amount = allowance.Amount
            };
        }
    }
}
