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

        public async Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
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
            await SaveFunction(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveManyAsync(List<T> entities)
        {
            foreach (var entity in entities)
            {
                await SaveFunction(entity);
            }

            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public async Task ChangeManyAsync(
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null)
        {
            if (added != null)
            {
                added.ForEach(entity =>
                {
                    _context.Entry(entity).State = EntityState.Added;
                });
            }

            if (updated != null)
            {
                updated.ForEach(entity =>
                {
                    _context.Entry(entity).State = EntityState.Modified;
                });
            }

            if (deleted != null)
            {
                deleted = deleted
                    .GroupBy(x => x.RowID)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                _context.Set<T>().RemoveRange(deleted);
            }

            await _context.SaveChangesAsync();
        }

        protected virtual void DetachNavigationProperties(T entity)
        {
            // no action
        }

        private async Task SaveFunction(T entity)
        {
            if (entity.IsNewEntity)
            {
                await CreateAsync(entity);
            }
            else
            {
                await UpdateAsync(entity);
            }
        }
    }
}
