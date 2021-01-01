using AccuPay.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Repositories
{
    public class FileRepository
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