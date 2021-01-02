using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISystemInfoRepository
    {
        Task<string> GetDesktopVersion();
    }
}
