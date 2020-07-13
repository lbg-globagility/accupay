using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class AllowanceDataService : BaseDataService<Allowance>
    {
        private readonly AllowanceRepository _repository;
        private readonly PayrollContext _context;

        public AllowanceDataService(AllowanceRepository repository, PayrollContext context) : base(repository)
        {
            _repository = repository;
            _context = context;
        }

        public async Task DeleteAsync(int allowanceId)
        {
            var allowance = await _repository.GetByIdAsync(allowanceId);

            if (allowance == null)
                throw new BusinessLogicException("Allowance does not exists.");

            await _repository.DeleteAsync(allowance);
        }

        protected override async Task SanitizeEntity(Allowance allowance)
        {
            if (allowance.IsOneTime)
                allowance.EffectiveEndDate = allowance.EffectiveStartDate;

            if (allowance.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (allowance.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (_repository.GetFrequencyList().Contains(allowance.AllowanceFrequency) == false)
                throw new BusinessLogicException("Invalid frequency.");

            if (allowance.ProductID == null)
                throw new BusinessLogicException("Allowance type is required.");

            if (allowance.EffectiveEndDate != null && allowance.EffectiveStartDate > allowance.EffectiveEndDate)
                throw new BusinessLogicException("Start date cannot be greater than end date.");

            if (allowance.Amount < 0)
                throw new BusinessLogicException("Amount cannot be less than 0.");

            var product = await _context.Products
                                        .Where(p => p.RowID == allowance.ProductID)
                                        .FirstOrDefaultAsync();

            if (product == null)
                throw new BusinessLogicException("The selected allowance type no longer exists.");

            if (allowance.IsMonthly && !product.Fixed)
                throw new BusinessLogicException("Only fixed allowance type are allowed for Monthly allowances.");
        }

        #region Queries

        public async Task<Allowance> GetByIdAsync(int allowanceId)
        {
            return await _repository.GetByIdAsync(allowanceId);
        }

        public async Task<Allowance> GetByIdWithEmployeeAndProductAsync(int allowanceId)
        {
            return await _repository.GetByIdWithEmployeeAndProductAsync(allowanceId);
        }

        public async Task<IEnumerable<Allowance>> GetByEmployeeWithProductAsync(int employeeId)
        {
            return await _repository.GetByEmployeeWithProductAsync(employeeId);
        }

        public async Task<PaginatedList<Allowance>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);
        }

        public List<string> GetFrequencyList()
        {
            return _repository.GetFrequencyList();
        }

        public async Task<bool> CheckIfAlreadyUsedAsync(int allowanceId)
        {
            return await _repository.CheckIfAlreadyUsedAsync(allowanceId);
        }

        #endregion Queries
    }
}