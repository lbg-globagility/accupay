using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Allowances.Models;
using AccuPay.Web.Employees.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Allowances.Services
{
    class AllowanceService
    {
        private readonly AllowanceRepository _allowanceRepository;

        public AllowanceService(AllowanceRepository allowanceRepository)
        {
            _allowanceRepository = allowanceRepository;
        }

        internal async Task<Allowance> Create(AllowanceDto dto)
        {
            var allowance = new Allowance();

            allowance.OrganizationID = 5;
            allowance.CreatedBy = 1;

            ApplyUpdate(dto, allowance);

            await Save(allowance);

            return allowance;
        }

        internal async Task Update(int id, AllowanceDto dto)
        {
            var allowance = await GetAllowanceById(id);

            allowance.LastUpdBy = 1;

            ApplyUpdate(dto, allowance);

            await Save(allowance);
        }

        internal async Task Delete(int id, AllowanceDto dto)
        {
            await _allowanceRepository.DeleteAsync(id);
        }

        private async Task Save(Allowance allowance)
        {
            List<Allowance> allowances = new List<Allowance>() { allowance };
            await _allowanceRepository.SaveManyAsync(allowances);
        }

        private static void ApplyUpdate(AllowanceDto dto, Allowance allowance)
        {
            allowance.RowID = dto.RowID;
            allowance.EmployeeID = dto.EmployeeID;
            allowance.ProductID = dto.ProductID;
            allowance.EffectiveStartDate = dto.EffectiveStartDate;
            allowance.AllowanceFrequency = dto.AllowanceFrequency;
            allowance.EffectiveEndDate = dto.EffectiveEndDate;
            allowance.TaxableFlag = dto.TaxableFlag;
            allowance.Amount = dto.Amount;
        }

        private async Task<Allowance> GetAllowanceById(int id)
        {
            return await _allowanceRepository.GetByIdAsync(id);
        }

        internal async Task<AllowanceDto> GeyByIdAsync(int id)
        {
            var allowance = await GetAllowanceById(id);
            var dto = AllowanceDto.Produce(allowance);

            return dto;
        }


    }
}
