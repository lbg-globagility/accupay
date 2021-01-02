using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IClientRepository
    {
        Task Create(Client client);

        Task<Client> GetById(int id);

        Task<(ICollection<Client> clients, int total)> List(PageOptions options);

        Task Update(Client client);
    }
}
