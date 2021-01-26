using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IListOfValueDataService : IBaseSavableDataService<ListOfValue>
    {
        Task CreateOrUpdateDefaultPayPeriod(int organizationId, int currentlyLoggedInUserId, TimePeriod firstHalf, TimePeriod endOfTheMonth);

        Task SaveDefaultPayPeriodData(int organizationId, int currentlyLoggedInUserId, TimePeriod firstHalf, TimePeriod endOfTheMonth);

        void ValidateDefaultPayPeriodData(TimePeriod firstHalf, TimePeriod endOfTheMonth);
    }
}
