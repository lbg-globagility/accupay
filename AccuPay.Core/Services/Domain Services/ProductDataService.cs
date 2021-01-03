using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using System;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class ProductDataService : IProductDataService
    {
        private readonly IAllowanceDataService _allowanceService;
        private readonly IProductRepository _productRepository;
        private readonly IAllowanceRepository _allowanceRepository;

        public ProductDataService(
            IProductRepository productRepository,
            IAllowanceRepository allowanceRepository,
            // TODO: Urgent - Data Service cannot have a dependency to another data service.
            IAllowanceDataService allowanceService)
        {
            _productRepository = productRepository;
            _allowanceService = allowanceService;
            _allowanceRepository = allowanceRepository;
        }

        public async Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType)
        {
            return await _productRepository.CheckIfAlreadyUsedInAllowancesAsync(allowanceType);
        }
    }
}
