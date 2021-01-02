using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISystemOwnerService
    {
        string GetCurrentSystemOwner();

        Task<string> GetCurrentSystemOwnerAsync();
    }
}
