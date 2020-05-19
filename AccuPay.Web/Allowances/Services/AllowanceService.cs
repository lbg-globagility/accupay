using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Allowances.Models;
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

        public async Task<AllowanceDto> GetByIdAsync(int id)
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
                EmployeeID = dto.EmployeeID
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
            allowance.ProductID = dto.ProductID;
            allowance.AllowanceFrequency = dto.AllowanceFrequency;
            allowance.EffectiveStartDate = dto.EffectiveStartDate;
            allowance.EffectiveEndDate = dto.EffectiveEndDate;
            allowance.Amount = dto.Amount;
        }

        private static AllowanceDto ConvertToDto(Allowance allowance)
        {
            if (allowance == null) return null;

            return new AllowanceDto()
            {
                Id = allowance.RowID.Value,
                EmployeeID = allowance.EmployeeID.Value,
                ProductID = allowance.ProductID.Value,
                EffectiveStartDate = allowance.EffectiveStartDate,
                AllowanceFrequency = allowance.AllowanceFrequency,
                EffectiveEndDate = allowance.EffectiveEndDate,
                Amount = allowance.Amount
            };
        }
    }
}
