using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class OrganizationRepository
    {
        private readonly PayrollContext _context;

        public OrganizationRepository(PayrollContext context)
        {
            _context = context;
        }

        public Organization GetById(int id)
        {
            return _context.Organizations.FirstOrDefault(x => x.RowID == id);
        }

        public async Task<Organization> GetFirst(int clientId)
        {
            return await _context.Organizations
                .Where(o => o.ClientId == clientId)
                .Where(o => o.IsInActive == false)
                .FirstOrDefaultAsync();
        }

        public async Task<Organization> GetByIdAsync(int id)
        {
            return await _context.Organizations.FirstOrDefaultAsync(x => x.RowID == id);
        }

        public async Task Create(Organization organization)
        {
            _context.Organizations.Add(organization);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Organization organization)
        {
            _context.Entry(organization).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<(ICollection<Organization> organizations, int total)> List(OrganizationPageOptions options, int clientId)
        {
            var query = _context.Organizations
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
    }
}