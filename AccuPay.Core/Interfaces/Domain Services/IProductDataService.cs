using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProductDataService
    {
        Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType);
    }
}
