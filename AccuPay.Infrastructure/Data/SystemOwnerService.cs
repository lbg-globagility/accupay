using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class SystemOwnerService : ISystemOwnerService
    {
        private readonly PayrollContext _context;

        public SystemOwnerService(PayrollContext context)
        {
            _context = context;
        }

        public string GetCurrentSystemOwner()
        {
            return GetCurrentSystemOwnerBaseQuery()
                .FirstOrDefault();
        }

        public async Task<string> GetCurrentSystemOwnerAsync()
        {
            return await GetCurrentSystemOwnerBaseQuery().
                            FirstOrDefaultAsync();
        }

        private IQueryable<string> GetCurrentSystemOwnerBaseQuery()
        {
            return _context.SystemOwners
                    .Where(x => x.IsCurrentOwner == "1")
                    .Select(x => x.Name);
        }
    }
}
