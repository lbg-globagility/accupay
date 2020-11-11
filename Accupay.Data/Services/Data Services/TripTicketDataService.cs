using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Data.Services.Data_Services
{
    public class TripTicketDataService
    {
        private readonly TripTicketRepository _tripTicketRepository;

        public TripTicketDataService(TripTicketRepository tripTicketRepository)
        {
            _tripTicketRepository = tripTicketRepository;
        }

        public async Task ImportAsync(List<TripTicket> tripTickets, List<Route> routes, List<Vehicle> vehicles, int organizationId, int userId)
        {
            await _tripTicketRepository.SaveImportAsync(tripTickets, routes, vehicles, organizationId, userId);
        }
    }
}