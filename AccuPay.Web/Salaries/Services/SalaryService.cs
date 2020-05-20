using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Salaries.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Salaries.Services
{
    public class SalaryService
    {
        private readonly SalaryRepository _repository;

        public SalaryService(SalaryRepository salaryRepository)
        {
            _repository = salaryRepository;
        }

        public async Task<PaginatedList<SalaryDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2; // temporary OrganizationID
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<SalaryDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<SalaryDto> GetById(int id)
        {
            var salary = await _repository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> Create(CreateSalaryDto dto)
        {
            var salary = new Salary
            {
                OrganizationID = 5,
                EmployeeID = dto.EmployeeId,
                CreatedBy = 1
            };

            ApplyChanges(dto, salary);

            await _repository.SaveAsync(salary);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> Update(int id, UpdateSalaryDto dto)
        {
            var salary = await _repository.GetByIdAsync(id);
            if (salary == null) return null;

            salary.LastUpdBy = 1;

            ApplyChanges(dto, salary);

            await _repository.SaveAsync(salary);

            return ConvertToDto(salary);
        }

        private static void ApplyChanges(ICrudSalaryDto dto, Salary salary)
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
    }
}
