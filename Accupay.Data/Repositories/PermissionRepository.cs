using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PermissionRepository
    {
        private readonly PayrollContext _context;

        public PermissionRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Permission>> GetAll()
        {
            return await _context.Permissions.ToListAsync();
        }
    }
}
