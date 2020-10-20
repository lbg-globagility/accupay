using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Services;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OrganizationRepository : SavableRepository<Organization>
    {
        public OrganizationRepository(PayrollContext context) : base(context)
        {
        }

        protected override void DetachNavigationProperties(Organization organization)
        {
            if (organization.Address != null)
            {
                _context.Entry(organization.Address).State = EntityState.Detached;
            }
        }

        public async Task<bool> CheckIfNameExistsAsync(string name, int? id)
        {
            var query = _context.Organizations
                .Where(x => x.Name.Trim().ToLower() == name.ToTrimmedLowerCase());

            if (id != null)
            {
                query = query.Where(x => x.RowID != id);
            }

            return await query.AnyAsync();
        }

        public async Task<Organization> GetByIdWithAddressAsync(int id)
        {
            return await _context.Organizations
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async Task<Organization> GetFirst(int clientId)
        {
            return await _context.Organizations
                .Where(o => o.ClientId == clientId)
                .Where(o => o.IsInActive == false)
                .FirstOrDefaultAsync();
        }

        public async Task<(ICollection<Organization> organizations, int total)> List(OrganizationPageOptions options, int clientId)
        {
            var query = _context.Organizations
                .AsNoTracking()
                .Where(o => o.IsInActive == false);

            if (options.HasClientId)
            {
                query = query.Where(t => t.ClientId == options.ClientId);
            }
            else
            {
                query = query.Where(t => t.ClientId == clientId);
            }

            var organizations = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (organizations, total);
        }

        public async Task<ICollection<UserRoleData>> GetUserRolesAsync(int organizationId)
        {
            IQueryable<UserRoleData> query = UserRoleQueryHelper.GetBaseQuery(_context);

            return await query.Where(x => x.OrganizationId == organizationId).ToListAsync();
        }
    }
}