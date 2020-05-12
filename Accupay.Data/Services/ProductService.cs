using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using System;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class ProductService
    {
        private readonly AllowanceRepository _allowanceRepository;
        private readonly ProductRepository _productRepository;

        public ProductService(AllowanceRepository allowanceRepository,
                            ProductRepository productRepository)
        {
            _allowanceRepository = allowanceRepository;
            _productRepository = productRepository;
        }

        public async Task<Allowance> GetOrCreateEmployeeEcola(int employeeId,
                                                            int organizationId,
                                                            int userId,
                                                            TimePeriod timePeriod,
                                                            string allowanceFrequency = Allowance.FREQUENCY_SEMI_MONTHLY,
                                                            decimal amount = 0)
        {
            var ecolaAllowance = await _allowanceRepository.GetEmployeeEcolaAsync(employeeId: employeeId,
                                                                            organizationId: organizationId,
                                                                            timePeriod: timePeriod);

            if (ecolaAllowance == null)
            {
                var ecolaProductId = (await _productRepository.GetOrCreateAllowanceTypeAsync(ProductConstant.ECOLA,
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

                await _allowanceRepository.SaveAsync(ecolaAllowance);

                ecolaAllowance = await _allowanceRepository.GetEmployeeEcolaAsync(employeeId: employeeId,
                                                                            organizationId: organizationId,
                                                                            timePeriod: timePeriod);
            }

            return ecolaAllowance;
        }
    }
}