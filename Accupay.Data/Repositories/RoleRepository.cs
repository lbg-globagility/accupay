using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class RoleRepository
    {
        private readonly PayrollContext _context;

        public RoleRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<AspNetRole> GetById(Guid roleId)
        {
            var role = await _context.Roles.Include(t => t.RolePermissions)
                .FirstOrDefaultAsync(t => t.Id == roleId);

            return role;
        }

        public async Task<(ICollection<AspNetRole> roles, int total)> List(PageOptions options, int clientId)
        {
            var query = _context.Roles
                .Where(t => t.ClientId == clientId);

            var roles = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (roles, total);
        }
    }
}
