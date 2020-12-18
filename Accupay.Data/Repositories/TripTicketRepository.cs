using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
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

        public async Task<ICollection<TripTicket>> GetByDateRangeAsync(TimePeriod datePeriod)
        {
            // TODO: check if there should be Organization filter here
            var tripTickets = await _context.TripTickets
                .Include(t => t.Employees)
                .Where(t => datePeriod.Start <= t.Date && t.Date <= datePeriod.End)
                .ToListAsync();

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
            tripTicket.Employees.ToList().ForEach(t => _context.Entry(t.Employee.Position).State = EntityState.Detached);

            //tripTicketEmployees.ToList().ForEach(t =>
            //{
            //    //if (t.IsNew)
            //    //{
            //    //    _context.TripTicketEmployees.Add()
            //    //}
            //})

            _context.SaveChanges();
        }

        internal async Task SaveImportAsync(List<TripTicket> tripTickets, List<Route> routes, List<Vehicle> vehicles, int organizationId, int userId)
        {
            _context.Routes.AddRange(routes);

            _context.Vehicles.AddRange(vehicles);

            await _context.SaveChangesAsync();

            foreach (var tt in tripTickets)
            {
                tt.RouteID = routes.FirstOrDefault(r => r.Description == tt.RouteDescription).RowID;
                tt.Route = null;

                tt.VehicleID = vehicles.FirstOrDefault(v => v.PlateNo == tt.VehiclePlateNo).RowID;
                tt.Vehicle = null;
            }
            _context.TripTickets.AddRange(tripTickets);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TripTicket>> GetAllAsync(int organizationId)
        {
            var builder = new TripTicketQueryBuilder(_context);
            return await builder.
                ToListAsync(organizationId);
        }

        public async Task<IEnumerable<TripTicket>> GetByEmployeeIDsByRouteByVehicleBetweenDatesAsync(int organizationId, List<string> routeDesciptions, List<string> vehicleDescriptions, TimePeriod tripDates)
        {
            var builder = new TripTicketQueryBuilder(_context);
            return await builder.
                IncludeRoute().
                IncludeVehicle().
                IncludeTripTicketEmployees().
                BetweeDates(tripDates).
                SimilarToRoute(routeDesciptions).
                SimilarToVehicle(vehicleDescriptions).
                ToListAsync(organizationId);
        }
    }
}
