using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITripTicketDataService
    {
        Task ImportAsync(List<TripTicket> tripTickets, List<Route> routes, List<Vehicle> vehicles, int organizationId, int currentlyLoggedInUserId);
    }
}
