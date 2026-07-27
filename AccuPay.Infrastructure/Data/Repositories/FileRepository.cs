using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task Update(File file)
        {
            _context.Entry(file).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<File> GetById(int id)
        {
            return await _context.Files.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
