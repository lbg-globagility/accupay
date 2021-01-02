using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class TripTicketQueryBuilder : ITripTicketQueryBuilder
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

        public ITripTicketQueryBuilder BetweeDates(TimePeriod timePeriod)
        {
            _query = _query
               .Where(tt => tt.Date >= timePeriod.Start)
               .Where(tt => tt.Date <= timePeriod.End);
            return this;
        }

        public ITripTicketQueryBuilder SimilarToRoute(List<string> routeDescriptions)
        {
            _query = _query
               .Where(tt => routeDescriptions.Any(name => IsEqualToLower(tt.Route.Description, name)));
            return this;
        }

        public ITripTicketQueryBuilder SimilarToVehicle(List<string> vehicleDescriptions)
        {
            _query = _query
               .Where(tt => vehicleDescriptions.Any(name => IsEqualToLower(tt.Vehicle.PlateNo, name)));
            return this;
        }

        private bool IsEqualToLower(string dataText, string comparedText) => dataText.ToLower() == comparedText.ToLower();

        public ITripTicketQueryBuilder IncludeRoute()
        {
            _query = _query.Include(x => x.Route);
            return this;
        }

        public ITripTicketQueryBuilder IncludeVehicle()
        {
            _query = _query.Include(x => x.Vehicle);
            return this;
        }

        public ITripTicketQueryBuilder IncludeTripTicketEmployees()
        {
            _query = _query
                .Include(x => x.Employees)
                .ThenInclude(tte => tte.Employee);
            return this;
        }
    }
}
