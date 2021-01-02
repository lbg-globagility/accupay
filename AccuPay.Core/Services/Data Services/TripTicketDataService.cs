using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class TripTicketDataService
    {
        private const string UserActivityName = "Trip Ticket";

        private readonly ITripTicketRepository _tripTicketRepository;
        private readonly IUserActivityRepository _userActivityRepository;

        public TripTicketDataService(
            ITripTicketRepository tripTicketRepository,
            IUserActivityRepository userActivityRepository)
        {
            _tripTicketRepository = tripTicketRepository;
            _userActivityRepository = userActivityRepository;
        }

        public async Task ImportAsync(
            List<TripTicket> tripTickets,
            List<Route> routes,
            List<Vehicle> vehicles,
            int organizationId,
            int currentlyLoggedInUserId)
        {
            await _tripTicketRepository.SaveImportAsync(
                tripTickets,
                routes,
                vehicles,
                organizationId,
                currentlyLoggedInUserId);

            await RecordAddMany(tripTickets, organizationId, currentlyLoggedInUserId);
        }

        private async Task RecordAddMany(List<TripTicket> tripTickets, int organizationId, int currentlyLoggedInUserId)
        {
            var activityItems = new List<UserActivityItem>();

            foreach (var tripTicket in tripTickets)
            {
                activityItems.Add(new UserActivityItem()
                {
                    EntityId = tripTicket.RowID.Value,
                    Description = $"Created a new trip ticket.",
                });
            }

            await _userActivityRepository.CreateRecordAsync(
                currentlyLoggedInUserId,
                UserActivityName,
                organizationId,
                UserActivity.RecordTypeAdd,
                activityItems);
        }
    }
}
