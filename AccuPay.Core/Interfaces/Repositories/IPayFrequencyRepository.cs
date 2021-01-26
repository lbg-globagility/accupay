using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPayFrequencyRepository
    {
        ICollection<PayFrequency> GetAll();

        Task<ICollection<PayFrequency>> GetAllAsync();
    }
}
