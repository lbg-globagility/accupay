using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class SystemInfoRepository
    {
        private readonly PayrollContext _context;

        public SystemInfoRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<string> GetDesktopVersion()
        {
            return (await _context.SystemInfo
                .AsNoTracking()
                .Where(x => x.Name == SystemInfo.DesktopVersion)
                .FirstOrDefaultAsync())?
                .Value;
        }
    }
}
