using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class SavableRepository<T> : BaseRepository where T : BaseEntity
    {
        protected readonly PayrollContext _context;

        public SavableRepository(PayrollContext context)
        {
            _context = context;
        }

        internal async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.RowID == id);
        }

        internal async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveAsync(T entity)
        {
            SaveFunction(entity);
            await _context.SaveChangesAsync();
        }

        internal async Task SaveManyAsync(List<T> entities)
        {
            entities.ForEach(entity => SaveFunction(entity));
            await _context.SaveChangesAsync();
        }

        protected virtual void DetachNavigationProperties(T entity)
        {
            // no action
        }

        private void SaveFunction(T entity)
        {
            if (IsNewEntity(entity.RowID))
            {
                _context.Set<T>().Add(entity);
            }
            else
            {
                _context.Entry(entity).State = EntityState.Modified;
            }

            DetachNavigationProperties(entity);
        }
    }
}