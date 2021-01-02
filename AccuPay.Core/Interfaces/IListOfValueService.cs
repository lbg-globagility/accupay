using AccuPay.Core.Entities;
using AccuPay.Core.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IListOfValueService
    {
        ListOfValueCollection Create(string type = null);

        ListOfValueCollection Create(IEnumerable<ListOfValue> values);

        Task<ListOfValueCollection> CreateAsync(string type = null);
    }
}
