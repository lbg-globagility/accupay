using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Salaries.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Salaries.Services
{
    public class SalaryService
    {
        private readonly SalaryRepository _repository;
        private readonly ICurrentUser _currentUser;

        public SalaryService(
            SalaryRepository salaryRepository,
            ICurrentUser currentUser)
        {
            _repository = salaryRepository;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<SalaryDto>> List(PageOptions options, string searchTerm, int employeeId)
        {
            // TODO: sort and desc in repository
            var paginatedList = await _repository.List(
                options,
                _currentUser.OrganizationId,
                searchTerm,
                employeeId);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<SalaryDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
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
                CreatedBy = _currentUser.DesktopUserId
            };

            ApplyChanges(dto, salary);

            await _repository.SaveAsync(salary);

            return ConvertToDto(salary);
        }

        public async Task<SalaryDto> Update(int id, UpdateSalaryDto dto)
        {
            var salary = await _repository.GetByIdAsync(id);
            if (salary == null) return null;

            salary.LastUpdBy = _currentUser.DesktopUserId;

            ApplyChanges(dto, salary);

            await _repository.SaveAsync(salary);

            return ConvertToDto(salary);
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
    }
}
