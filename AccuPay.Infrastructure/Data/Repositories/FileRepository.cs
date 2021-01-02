using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class FileRepository : IFileRepository
    {
        private readonly PayrollContext _context;

        public FileRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task Create(File file)
        {
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
        }
    }
}
