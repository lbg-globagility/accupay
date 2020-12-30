using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public interface IBaseEmployeeDataService<T> where T : EmployeeDataEntity
    {
        Task DeleteAsync(int id, int currentlyLoggedInUserId);

        Task<T> SaveAsync(T entity, int currentlyLoggedInUserId);

        Task SaveManyAsync(int currentlyLoggedInUserId, List<T> added = null, List<T> updated = null, List<T> deleted = null);

        Task SaveManyAsync(List<T> entities, int currentlyLoggedInUserId);
    }
}
