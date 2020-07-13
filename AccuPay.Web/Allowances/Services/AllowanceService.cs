using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Products;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Allowances.Services
{
    public class AllowanceService
    {
        private readonly ProductRepository _productRepository;
        private readonly AllowanceDataService _allowanceService;
        private readonly ICurrentUser _currentUser;

        public AllowanceService(
            AllowanceDataService allowanceService,
            ProductRepository productRepository,
            ICurrentUser currentUser)
        {
            _productRepository = productRepository;
            _allowanceService = allowanceService;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<AllowanceDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _allowanceService.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId,
                searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<AllowanceDto> GetById(int id)
        {
            var allowance = await _allowanceService.GetByIdWithEmployeeAndProductAsync(id);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Create(CreateAllowanceDto dto)
        {
            var allowance = new Allowance
            {
                OrganizationID = _currentUser.OrganizationId,
                CreatedBy = _currentUser.DesktopUserId,
                EmployeeID = dto.EmployeeId
            };
            ApplyChanges(dto, allowance);

            await _allowanceService.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Update(int id, UpdateAllowanceDto dto)
        {
            var allowance = await _allowanceService.GetByIdAsync(id);
            if (allowance == null) return null;

            allowance.LastUpdBy = _currentUser.DesktopUserId;

            ApplyChanges(dto, allowance);

            await _allowanceService.SaveAsync(allowance);

            return ConvertToDto(allowance);
        }

        public async Task Delete(int id)
        {
            await _allowanceService.DeleteAsync(id);
        }

        public async Task<List<ProductDto>> GetAllowanceTypes()
        {
            var leaveTypes = await _productRepository.GetAllowanceTypesAsync(_currentUser.OrganizationId);

            return leaveTypes
                    .Where(x => !string.IsNullOrWhiteSpace(x.PartNo))
                    .Select(x => ProductDto.FromProduct(x))
                    .ToList();
        }

        public List<string> GetFrequencyList()
        {
            return _allowanceService.GetFrequencyList();
        }

        private static void ApplyChanges(CrudAllowanceDto dto, Allowance allowance)
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
