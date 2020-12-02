using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class ProductDataService
    {
        private readonly AllowanceDataService _allowanceService;
        private readonly ProductRepository _productRepository;
        private readonly AllowanceRepository _allowanceRepository;

        public ProductDataService(
            ProductRepository productRepository,
            AllowanceRepository allowanceRepository,
            AllowanceDataService allowanceService)
        {
            _productRepository = productRepository;
            _allowanceService = allowanceService;
            _allowanceRepository = allowanceRepository;
        }

        public async Task<Allowance> GetOrCreateEmployeeEcola(
            int employeeId,
            int organizationId,
            int userId,
            TimePeriod timePeriod,
            string allowanceFrequency = Allowance.FREQUENCY_SEMI_MONTHLY,
            decimal amount = 0)
        {
            var ecolaAllowance = await _allowanceRepository.GetEmployeeEcolaAsync(
                employeeId: employeeId,
                organizationId: organizationId,
                timePeriod: timePeriod);

            if (ecolaAllowance == null)
            {
                var ecolaProductId = (await _productRepository.GetOrCreateAllowanceTypeAsync(
                    ProductConstant.ECOLA,
                    organizationId,
                    userId))?.RowID;

                DateTime? effectiveEndDate = null;

                ecolaAllowance = new Allowance();
                ecolaAllowance.EmployeeID = employeeId;
                ecolaAllowance.ProductID = ecolaProductId;
                ecolaAllowance.AllowanceFrequency = allowanceFrequency;
                ecolaAllowance.EffectiveStartDate = timePeriod.Start;
                ecolaAllowance.EffectiveEndDate = effectiveEndDate;
                ecolaAllowance.Amount = amount;
                ecolaAllowance.CreatedBy = userId;
                ecolaAllowance.OrganizationID = organizationId;

                await _allowanceService.SaveAsync(ecolaAllowance);

                ecolaAllowance = await _allowanceRepository.GetEmployeeEcolaAsync(
                    employeeId: employeeId,
                    organizationId: organizationId,
                    timePeriod: timePeriod);
            }

            return ecolaAllowance;
        }

        public async Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType)
        {
            return await _productRepository.CheckIfAlreadyUsedInAllowancesAsync(allowanceType);
        }
    }
}
