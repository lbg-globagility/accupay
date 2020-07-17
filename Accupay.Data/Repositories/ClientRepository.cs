using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ClientRepository
    {
        private readonly PayrollContext _context;

        public ClientRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<Client> GetById(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            return client;
        }

        public async Task<(ICollection<Client> clients, int total)> List(PageOptions options)
        {
            var query = _context.Clients.AsQueryable();

            var clients = await query.Page(options).ToListAsync();
            var total = await query.CountAsync();

            return (clients, total);
        }

        public async Task Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Client client)
        {
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}