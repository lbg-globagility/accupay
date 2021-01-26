using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITripTicketQueryBuilder
    {
        List<TripTicket> ToList(int organizationId);

        Task<List<TripTicket>> ToListAsync(int? organizationId);

        ITripTicketQueryBuilder BetweeDates(TimePeriod timePeriod);

        ITripTicketQueryBuilder IncludeRoute();

        ITripTicketQueryBuilder IncludeTripTicketEmployees();

        ITripTicketQueryBuilder IncludeVehicle();

        ITripTicketQueryBuilder SimilarToRoute(List<string> routeDescriptions);

        ITripTicketQueryBuilder SimilarToVehicle(List<string> vehicleDescriptions);
    }
}
