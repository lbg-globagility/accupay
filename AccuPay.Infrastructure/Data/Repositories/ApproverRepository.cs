using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ApproverRepository : SavableRepository<Approver>, IApproverRepository
    {
        public ApproverRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Approver entity)
        {
            if (entity.Organization != null)
            {
                _context.Entry(entity.Organization).State = EntityState.Detached;
            }
        }

        public async Task<PaginatedList<Approver>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Approvers
                .Include(a => a.Organization)
                .Where(a => a.OrganizationID == organizationId)
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var term = $"%{searchTerm}%";

                query = query.Where(a =>
                    EF.Functions.Like(a.FirstName, term) ||
                    EF.Functions.Like(a.LastName, term) ||
                    EF.Functions.Like(a.CompanyName, term) ||
                    EF.Functions.Like(a.EmailAddress, term));
            }

            var items = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Approver>(items, count);
        }

        public async Task<Approver> GetByIdWithOrganizationAsync(int id)
        {
            return await _context.Approvers
                .Include(a => a.Organization)
                .FirstOrDefaultAsync(a => a.RowID == id);
        }
    }
}