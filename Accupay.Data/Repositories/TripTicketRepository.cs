using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TripTicketRepository
    {
        private readonly PayrollContext _context;

        public TripTicketRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<TripTicket> FindById(int? tripTicketId)
        {
            var tripTicket = await _context.TripTickets.FindAsync(tripTicketId);

            return tripTicket;
        }

        public async Task<ICollection<TripTicket>> GetAll()
        {
            var tripTickets = await _context.TripTickets.ToListAsync();

            return tripTickets;
        }

        public async Task<ICollection<TripTicketEmployee>> GetTripTicketEmployees(int? tripTicketId)
        {
            var tripTicketEmployees = await _context.TripTicketEmployees
                .Include(t => t.Employee)
                .Where(t => t.RowID == tripTicketId)
                .ToListAsync();

            return tripTicketEmployees;
        }
    }
}
