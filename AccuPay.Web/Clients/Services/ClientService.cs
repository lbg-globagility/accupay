using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Clients
{
    public class ClientService
    {
        private readonly ClientRepository _repository;

        public ClientService(ClientRepository repository)
        {
            _repository = repository;
        }

        public async Task<ClientDto> GetById(int clientId)
        {
            var client = await _repository.GetById(clientId);

            return ConvertToDto(client);
        }

        public async Task<PaginatedList<ClientDto>> List(PageOptions options)
        {
            var (clients, total) = await _repository.List(options);
            var dtos = clients.Select(t => ConvertToDto(t));

            return new PaginatedList<ClientDto>(dtos, total);
        }

        public async Task Create(CreateClientDto dto)
        {
        }

        public async Task Update(UpdateClientDto dto)
        {
        }

        private ClientDto ConvertToDto(Client client)
        {
            var dto = new ClientDto()
            {
                Id = client.Id,
                Name = client.Name,
                TradeName = client.TradeName,
                Address = client.Address,
                PhoneNumber = client.PhoneNumber,
                ContactPerson = client.ContactPerson,
            };

            return dto;
        }
    }
}
