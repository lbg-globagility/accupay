using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PayrollContext _context;

        public PermissionRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Permission>> GetAll(bool forDesktopOnly = false)
        {
            // TODO: delete Account Permission
            var query = _context.Permissions.AsQueryable();

            if (forDesktopOnly)
            {
                query = query.Where(x => x.Name != PermissionConstant.EMPLOYMENTPOLICY);
            }
            else
            {
                query = query.Where(x => x.ForDesktopOnly == false);
            }

            return await query.ToListAsync();
        }
    }
}
