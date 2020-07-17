using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public T GetById(int id)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(x => x.RowID == id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async Task<ICollection<T>> GetManyByIdsAsync(int[] ids)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.RowID.Value))
                .ToListAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync(T entity)
        {
            SaveFunction(entity, IsNewEntity(entity.RowID));
            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<T> entities)
        {
            entities.ForEach(entity => SaveFunction(entity, IsNewEntity(entity.RowID)));
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(T entity)
        {
            SaveFunction(entity, newEntity: true);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            SaveFunction(entity, newEntity: false);
            await _context.SaveChangesAsync();
        }

        protected virtual void DetachNavigationProperties(T entity)
        {
            // no action
        }

        private void SaveFunction(T entity, bool newEntity)
        {
            if (newEntity)
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