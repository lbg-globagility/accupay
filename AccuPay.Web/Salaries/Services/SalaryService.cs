using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Salaries.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Salaries.Services
{
    class SalaryService
    {
        private readonly SalaryRepository _salaryRepository;

        public SalaryService(SalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }

        internal async Task Update(int id, SalaryDto dto)
        {
            var salary = await GetSalaryById(id);

            salary.LastUpdBy = 1;

            ApplyUpdate(dto, salary);

            await Save(salary);
        }

        internal async Task<Salary> Create(SalaryDto dto)
        {
            var salary = new Salary();

            salary.OrganizationID = 5;
            salary.CreatedBy = 1;

            ApplyUpdate(dto, salary);

            await Save(salary);

            return salary;
        }

        internal async Task Delete(int id, SalaryDto dto)
        {
            await _salaryRepository.DeleteAsync(id);
        }

        private async Task Save(Salary salary)
        {
            List<Salary> salaries = new List<Salary>() { salary };
            await _salaryRepository.SaveManyAsync(salaries);
        }

        private static void ApplyUpdate(SalaryDto dto, Salary salary)
        {
            salary.EmployeeID = dto.EmployeeID;
            salary.FilingStatusID = dto.FilingStatusID;
            salary.PositionID = dto.PositionID;
            salary.PayPhilHealthID = dto.PayPhilHealthID;
            salary.PhilHealthDeduction = dto.PhilHealthDeduction;
            salary.HDMFAmount = dto.HDMFAmount;
            salary.BasicSalary = dto.BasicSalary;
            salary.AllowanceSalary = dto.AllowanceSalary;
            salary.TotalSalary = dto.TotalSalary;
            //salary.DailyRate = dto.DailyRate;
            //salary.HourlyRate = dto.HourlyRate;
            salary.NoOfDependents = dto.NoOfDependents;
            salary.MaritalStatus = dto.MaritalStatus;
            salary.EffectiveFrom = dto.EffectiveFrom;
            salary.EffectiveTo = dto.EffectiveTo;
            salary.DoPaySSSContribution = dto.DoPaySSSContribution;
            salary.AutoComputeHDMFContribution = dto.AutoComputeHDMFContribution;
            salary.AutoComputePhilHealthContribution = dto.AutoComputePhilHealthContribution;
        }

        private async Task<Salary> GetSalaryById(int id)
        {
            return await _salaryRepository.GetByIdAsync(id);
        }

        internal async Task<SalaryDto> GeyByIdAsync(int id)
        {
            var salary = await GetSalaryById(id);
            var dto = SalaryDto.Produce(salary);

            return dto;
        }
    }
}
