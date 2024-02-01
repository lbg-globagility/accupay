using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class ResetLeaveCreditRepository : SavableRepository<ResetLeaveCredit>, IResetLeaveCreditRepository
    {
        public ResetLeaveCreditRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<ICollection<ResetLeaveCredit>> GetResetLeaveCredits(int organizationId)
        {
            return await _context.ResetLeaveCredits
                .AsNoTracking()
                .Include(r => r.ResetLeaveCreditItems)
                    .ThenInclude(i => i.Employee)
                .Include(r => r.PayPeriod)
                .Where(r => r.OrganizationID == organizationId)
                .ToListAsync();
        }

        async Task ISavableRepository<ResetLeaveCredit>.SaveManyAsync(List<ResetLeaveCredit> added = null,
            List<ResetLeaveCredit> updated = null,
            List<ResetLeaveCredit> deleted = null)
        {
            if (added != null && added.Any())
            {
                //added.ForEach(a =>
                //{
                //    _context.Entry(a).State = EntityState.Added;
                //});
                _context.ResetLeaveCredits.AddRange(added);
            }

            if (updated != null && updated.Any())
            {
                var resetLeaveCredits = await ((ISavableRepository<ResetLeaveCredit>)this).GetManyByIdsAsync(updated.Select(u => u.RowID.Value).ToArray());

                resetLeaveCredits.ToList().ForEach(r =>
                {
                    var update = updated.FirstOrDefault(u => u.RowID == r.RowID);
                    if (update == null) return;

                    r.ResetLeaveCreditItems.ToList().ForEach(i =>
                    {
                        var updatedItem = update.ResetLeaveCreditItems.FirstOrDefault(ii => ii.RowID == i.RowID);
                        if (updatedItem == null) return;
                        i.Update(userId: updatedItem.LastUpdBy.Value,
                            vacationCredit: updatedItem.VacationLeaveCredit,
                            sickCredit: updatedItem.SickLeaveCredit,
                            isSelected: updatedItem.IsSelected,
                            isApplied: updatedItem.IsApplied);
                        _context.Entry(i).State = EntityState.Modified;
                    });
                });
            }

            if (deleted != null && deleted.Any())
            {
                //deleted.ForEach(d =>
                //{
                //    _context.Entry(d).State = EntityState.Deleted;
                //});
                _context.ResetLeaveCredits.RemoveRange(deleted);
            }

            await _context.SaveChangesAsync();
        }

        ResetLeaveCredit ISavableRepository<ResetLeaveCredit>.GetById(int id) => _context.ResetLeaveCredits
                .Include(r => r.ResetLeaveCreditItems)
                .Where(x => x.RowID == id)
                .FirstOrDefault();

        Task<ResetLeaveCredit> ISavableRepository<ResetLeaveCredit>.GetByIdAsync(int id)
        {
            var resetLeaveCredit = ((ISavableRepository<ResetLeaveCredit>)this).GetById(id);

            return Task.FromResult(resetLeaveCredit);
        }

        async Task<ICollection<ResetLeaveCredit>> ISavableRepository<ResetLeaveCredit>.GetManyByIdsAsync(int[] ids)
        {
            return await _context.ResetLeaveCredits
                .AsNoTracking()
                .Include(r => r.ResetLeaveCreditItems)
                .Where(r => ids.Contains(r.RowID.Value))
                .ToListAsync();
        }

        async Task ISavableRepository<ResetLeaveCredit>.DeleteAsync(ResetLeaveCredit entity)
        {
            if (entity.RowID == null) return;

            var resetLeaveCredit = ((ISavableRepository<ResetLeaveCredit>)this).GetById(entity.RowID.Value);

            if (resetLeaveCredit.HasApplications) throw new BusinessLogicException(message: "This can't be deleted since it has been already applied to the selected employee(s).");

            _context.ResetLeaveCreditItems.RemoveRange(resetLeaveCredit.ResetLeaveCreditItems);

            _context.ResetLeaveCredits.Remove(resetLeaveCredit);

            await _context.SaveChangesAsync();
        }

    }
}
