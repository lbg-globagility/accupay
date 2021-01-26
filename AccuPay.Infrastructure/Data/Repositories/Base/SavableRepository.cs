using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public abstract class SavableRepository<T> : BaseRepository, ISavableRepository<T> where T : BaseEntity
    {
        protected readonly PayrollContext _context;

        public SavableRepository(PayrollContext context)
        {
            _context = context;
        }

        public virtual T GetById(int id)
        {
            return _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(x => x.RowID == id);
        }

        public async virtual Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async virtual Task<ICollection<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async virtual Task<ICollection<T>> GetManyByIdsAsync(int[] ids)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.RowID.Value))
                .ToListAsync();
        }

        public async virtual Task DeleteAsync(T entity)
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task SaveAsync(T entity)
        {
            await SaveFunction(entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public async virtual Task UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            DetachNavigationProperties(entity);

            await _context.SaveChangesAsync();
        }

        public async virtual Task SaveManyAsync(List<T> entities)
        {
            await SaveManyAsync(
                added: entities.Where(x => x.IsNewEntity).ToList(),
                updated: entities.Where(x => !x.IsNewEntity).ToList());
        }

        public async virtual Task SaveManyAsync(
            List<T> added = null,
            List<T> updated = null,
            List<T> deleted = null)
        {
            if (added != null)
            {
                added.ForEach(entity =>
                {
                    _context.Entry(entity).State = EntityState.Added;
                    DetachNavigationProperties(entity);
                });
            }

            if (updated != null)
            {
                updated.ForEach(entity =>
                {
                    _context.Entry(entity).State = EntityState.Modified;
                    DetachNavigationProperties(entity);
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
