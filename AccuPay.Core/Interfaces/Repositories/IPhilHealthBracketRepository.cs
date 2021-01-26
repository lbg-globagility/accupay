using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IPhilHealthBracketRepository
    {
        IEnumerable<PhilHealthBracket> GetAll();

        Task<IEnumerable<PhilHealthBracket>> GetAllAsync();
    }
}
