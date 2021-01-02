using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISavableRepository<T> : IBaseRepository where T : BaseEntity
    {
        Task CreateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<ICollection<T>> GetAllAsync();

        T GetById(int id);

        Task<T> GetByIdAsync(int id);

        Task<ICollection<T>> GetManyByIdsAsync(int[] ids);

        Task SaveAsync(T entity);

        Task SaveManyAsync(List<T> added = null, List<T> updated = null, List<T> deleted = null);

        Task SaveManyAsync(List<T> entities);

        Task UpdateAsync(T entity);
    }
}
