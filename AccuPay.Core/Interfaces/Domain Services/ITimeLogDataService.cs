using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeLogDataService : IBaseSavableDataService<TimeLog>
    {
        Task SaveImportAsync(List<TimeLog> timeLogs, int changedByUserId);
    }
}
