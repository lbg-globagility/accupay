using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IFileRepository
    {
        Task Create(File file);
    }
}
