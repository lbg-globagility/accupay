using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Core.Services.Imports.Salaries;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Salaries.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using static AccuPay.Core.Services.Imports.Salaries.SalaryImportParser;

namespace AccuPay.Web.Salaries.Services
{
    public class SalaryService
    {
        private readonly SalaryRepository _repository;
        private readonly SalaryDataService _dataService;
        private readonly SalaryImportParser _importParser;
        private readonly ICurrentUser _currentUser;

        public SalaryService(
            SalaryRepository repository,
            SalaryDataService dataService,
            ICurrentUser currentUser,
            SalaryImportParser salaryImportParser)
        {
            _repository = repository;
            _dataService = dataService;
            _currentUser = currentUser;
            _importParser = salaryImportParser;
        }

        public async Task<PaginatedList<SalaryDto>> List(PageOptions options, string searchTerm, int employeeId)
        {
            // TODO: sort and desc in repository
            var paginatedList = await _repository.List(
                options,
                _currentUser.OrganizationId,
                searchTerm,
                employeeId);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<SalaryDto> GetById(int id)
        {
            var salary = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> GetLatest(int employeeId)
        {
            var salary = await _repository.GetLatest(employeeId);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> Create(CreateSalaryDto dto)
        {
            var salary = new Salary
            {
                OrganizationID = _currentUser.OrganizationId,
                EmployeeID = dto.EmployeeId,
            };

            ApplyChanges(dto, salary);

            await _dataService.SaveAsync(salary, _currentUser.UserId);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> Update(int id, UpdateSalaryDto dto)
        {
            var salary = await _repository.GetByIdAsync(id);
            if (salary == null) return null;

            ApplyChanges(dto, salary);

            await _dataService.SaveAsync(salary, _currentUser.UserId);

            return ConvertToDto(salary);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                id: id,
                currentlyLoggedInUserId: _currentUser.UserId);
        }

        private static void ApplyChanges(CrudSalaryDto dto, Salary salary)
        {
            salary.BasicSalary = dto.BasicSalary;
            salary.AllowanceSalary = dto.AllowanceSalary;
            salary.EffectiveFrom = dto.EffectiveFrom;
            salary.DoPaySSSContribution = dto.DoPaySSSContribution;
            salary.AutoComputePhilHealthContribution = dto.AutoComputePhilHealthContribution;
            salary.PhilHealthDeduction = dto.PhilHealthDeduction;
            salary.AutoComputeHDMFContribution = dto.AutoComputeHDMFContribution;
            salary.HDMFAmount = dto.HDMFDeduction;
        }

        internal static SalaryDto ConvertToDto(Salary salary)
        {
            if (salary == null) return null;

            return new SalaryDto()
            {
                Id = salary.RowID.Value,
                EmployeeId = salary.EmployeeID.Value,
                EmployeeNumber = salary.Employee?.EmployeeNo,
                EmployeeName = salary.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = salary.Employee?.EmployeeType,
                BasicSalary = salary.BasicSalary,
                AllowanceSalary = salary.AllowanceSalary,
                TotalSalary = salary.TotalSalary,
                EffectiveFrom = salary.EffectiveFrom,
                DoPaySSSContribution = salary.DoPaySSSContribution,
                AutoComputePhilHealthContribution = salary.AutoComputePhilHealthContribution,
                PhilHealthDeduction = salary.PhilHealthDeduction,
                AutoComputeHDMFContribution = salary.AutoComputeHDMFContribution,
                HDMFDeduction = salary.HDMFAmount
            };
        }

        internal async Task<SalaryImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _dataService.BatchApply(
                parsedResult.ValidRecords,
                organizationId: _currentUser.OrganizationId,
                currentlyLoggedInUserId: userId);

            return parsedResult;
        }
    }
}
