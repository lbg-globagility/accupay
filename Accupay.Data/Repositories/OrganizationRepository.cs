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

        public async Task<(ICollection<Organization> organizations, int total)> List(PageOptions options)
        {
            var query = _context.Organizations.AsQueryable();

            var organizations = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (organizations, total);
        }
    }
}
