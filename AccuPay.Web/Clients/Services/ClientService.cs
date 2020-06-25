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

        public async Task<ClientDto> Create(CreateClientDto dto)
        {
            var client = new Client()
            {
                Name = dto.Name,
                TradeName = dto.TradeName,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                ContactPerson =  dto.ContactPerson,
            };

            await _repository.Create(client);

            return ConvertToDto(client);
        }

        public async Task<ClientDto> Update(int id, UpdateClientDto dto)
        {
            var client = await _repository.GetById(id);

            client.Name = dto.Name;
            client.TradeName = dto.TradeName;
            client.Address = dto.Address;
            client.PhoneNumber = dto.PhoneNumber;
            client.ContactPerson = dto.ContactPerson;

            await _repository.Update(client);

            return ConvertToDto(client);
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
