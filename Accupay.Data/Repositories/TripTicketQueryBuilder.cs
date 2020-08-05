using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class TripTicketQueryBuilder
    {
        private readonly PayrollContext _context;

        private IQueryable<TripTicket> _query;

        public TripTicketQueryBuilder(PayrollContext context)
        {
            _context = context;
            _query = _context.TripTickets;
        }

        public List<TripTicket> ToList(int organizationId)
        {
            return _query.Where(tt => tt.OrganizationID == organizationId).ToList();
        }

        public async Task<List<TripTicket>> ToListAsync(int? organizationId)
        {
            return await _query.Where(tt => tt.OrganizationID == organizationId).ToListAsync();
        }

        public TripTicketQueryBuilder BetweeDates(TimePeriod timePeriod)
        {
            _query = _query
               .Where(tt => tt.TripDate >= timePeriod.Start)
               .Where(tt => tt.TripDate <= timePeriod.End);
            return this;
        }

        public TripTicketQueryBuilder SimilarToRoute(List<string> routeDescriptions)
        {
            _query = _query
               .Where(tt => routeDescriptions.Any(name => IsEqualToLower(tt.Route.Description, name)));
            return this;
        }

        public TripTicketQueryBuilder SimilarToVehicle(List<string> vehicleDescriptions)
        {
            _query = _query
               .Where(tt => vehicleDescriptions.Any(name => IsEqualToLower(tt.Vehicle.PlateNo, name)));
            return this;
        }

        private bool IsEqualToLower(string dataText, string comparedText) => dataText.ToLower() == comparedText.ToLower();

        public TripTicketQueryBuilder IncludeRoute()
        {
            _query = _query.Include(x => x.Route);
            return this;
        }

        public TripTicketQueryBuilder IncludeVehicle()
        {
            _query = _query.Include(x => x.Vehicle);
            return this;
        }

        public TripTicketQueryBuilder IncludeTripTicketEmployees()
        {
            _query = _query
                .Include(x => x.TripTicketEmployees)
                .ThenInclude(tte => tte.Employee);
            return this;
        }
    }
}