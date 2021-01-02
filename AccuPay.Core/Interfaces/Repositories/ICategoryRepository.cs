using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category> GetByNameAsync(int organizationId, string categoryName);
    }
}
