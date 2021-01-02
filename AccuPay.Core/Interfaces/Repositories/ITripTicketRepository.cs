using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ITripTicketRepository
    {
        Task Create(TripTicket tripTicket);

        Task<TripTicket> FindById(int? tripTicketId);

        Task<ICollection<TripTicket>> GetAll();

        Task<IEnumerable<TripTicket>> GetAllAsync(int organizationId);

        Task<ICollection<TripTicket>> GetByDateRangeAsync(TimePeriod datePeriod);

        Task<IEnumerable<TripTicket>> GetByEmployeeIDsByRouteByVehicleBetweenDatesAsync(int organizationId, List<string> routeDesciptions, List<string> vehicleDescriptions, TimePeriod tripDates);

        Task<ICollection<TripTicketEmployee>> GetTripTicketEmployees(int? tripTicketId);

        Task SaveImportAsync(List<TripTicket> tripTickets, List<Route> routes, List<Vehicle> vehicles, int organizationId, int userId);

        void Update(TripTicket tripTicket);
    }
}
