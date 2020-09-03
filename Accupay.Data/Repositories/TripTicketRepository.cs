using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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

        public ICollection<TripTicket> GetByDateRange(DateTime from, DateTime to)
        {
            var tripTickets = _context.TripTickets
                .Include(t => t.Employees)
                .Where(t => from <= t.Date && t.Date <= to)
                .ToList();

            return tripTickets;
        }

        public async Task<ICollection<TripTicketEmployee>> GetTripTicketEmployees(int? tripTicketId)
        {
            var tripTicketEmployees = await _context.TripTicketEmployees
                .Include(t => t.Employee)
                .Where(t => t.TripTicketID == tripTicketId)
                .ToListAsync();

            return tripTicketEmployees;
        }

        public async Task Create(TripTicket tripTicket)
        {
            _context.TripTickets.Add(tripTicket);
            await _context.SaveChangesAsync();
        }

        public void Update(TripTicket tripTicket)
        {
            _context.Entry(tripTicket).State = EntityState.Modified;

            tripTicket.Employees.ToList().ForEach(t => _context.Entry(t.Employee).State = EntityState.Detached);

            //tripTicketEmployees.ToList().ForEach(t =>
            //{
            //    //if (t.IsNew)
            //    //{
            //    //    _context.TripTicketEmployees.Add()
            //    //}
            //})

            _context.SaveChanges();
        }
    }
}