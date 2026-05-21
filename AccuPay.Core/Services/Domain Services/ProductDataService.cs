using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class ProductDataService : IProductDataService
    {
        private readonly IProductRepository _productRepository;

        public ProductDataService(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType)
        {
            return await _productRepository.CheckIfAlreadyUsedInAllowancesAsync(allowanceType);
        }
    }
}
