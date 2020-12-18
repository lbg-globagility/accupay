using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Data.Services.Imports.Allowances;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Products;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Allowances.Services
{
    public class AllowanceService
    {
        private readonly ProductRepository _productRepository;
        private readonly AllowanceRepository _allowanceRepository;
        private readonly AllowanceDataService _dataService;
        private readonly ICurrentUser _currentUser;
        private readonly AllowanceImportParser _importParser;

        public AllowanceService(
            AllowanceDataService dataService,
            ProductRepository productRepository,
            AllowanceRepository allowanceRepository,
            ICurrentUser currentUser,
            AllowanceImportParser importParser)
        {
            _productRepository = productRepository;
            _allowanceRepository = allowanceRepository;
            _dataService = dataService;
            _currentUser = currentUser;
            _importParser = importParser;
        }

        public async Task<PaginatedList<AllowanceDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _allowanceRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId,
                searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<AllowanceDto> GetById(int id)
        {
            var allowance = await _allowanceRepository.GetByIdWithEmployeeAndProductAsync(id);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Create(CreateAllowanceDto dto)
        {
            var allowance = new Allowance
            {
                OrganizationID = _currentUser.OrganizationId,
                EmployeeID = dto.EmployeeId
            };
            ApplyChanges(dto, allowance);

            await _dataService.SaveAsync(allowance, _currentUser.UserId);

            return ConvertToDto(allowance);
        }

        internal async Task<AllowanceDto> Update(int id, UpdateAllowanceDto dto)
        {
            var allowance = await _allowanceRepository.GetByIdAsync(id);
            if (allowance == null) return null;

            ApplyChanges(dto, allowance);

            await _dataService.SaveAsync(allowance, _currentUser.UserId);

            return ConvertToDto(allowance);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                id: id,
                currentlyLoggedInUserId: _currentUser.UserId);
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
            return _allowanceRepository.GetFrequencyList();
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

            var allowanceDto = new AllowanceDto()
            {
                Id = allowance.RowID.Value,
                EmployeeId = allowance.EmployeeID.Value,
                EmployeeNumber = allowance.Employee?.EmployeeNo,
                EmployeeName = allowance.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = allowance.Employee?.EmployeeType,
                AllowanceType = allowance.Type,
                StartDate = allowance.EffectiveStartDate,
                Frequency = allowance.AllowanceFrequency,
                EndDate = allowance.EffectiveEndDate,
                Amount = allowance.Amount
            };

            if (!allowance.ProductID.HasValue && allowance.AllowanceTypeId.HasValue)
            {
                allowanceDto.AllowanceTypeId = allowance.AllowanceType.Id;
            }
            else if (allowance.ProductID.HasValue)
            {
                allowanceDto.AllowanceTypeId = allowance.ProductID.Value;
            }

            return allowanceDto;
        }

        internal async Task<AllowanceImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _dataService.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, currentlyLoggedInUserId: userId);

            return parsedResult;
        }
    }
}
