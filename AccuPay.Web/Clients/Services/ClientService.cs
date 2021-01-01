using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Users.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Clients
{
    public class ClientService
    {
        private readonly ClientRepository _clientRepository;
        private readonly UserManager<AspNetUser> _users;
        private readonly RoleManager<AspNetRole> _roles;
        private readonly UserEmailService _emailService;
        private OrganizationDataService _organizationDataService;
        private readonly ICurrentUser _currentUser;

        public ClientService(
            ClientRepository clientRepository,
            UserManager<AspNetUser> users,
            RoleManager<AspNetRole> roles,
            UserEmailService emailService,
            OrganizationDataService organizationDataService,
            ICurrentUser currentUser)
        {
            _clientRepository = clientRepository;
            _users = users;
            _roles = roles;
            _emailService = emailService;
            _organizationDataService = organizationDataService;
            _currentUser = currentUser;
        }

        public async Task<ClientDto> GetById(int clientId)
        {
            var client = await _clientRepository.GetById(clientId);

            return ConvertToDto(client);
        }

        public async Task<PaginatedList<ClientDto>> List(PageOptions options)
        {
            var (clients, total) = await _clientRepository.List(options);
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
                ContactPerson = dto.ContactPerson,
            };

            await _clientRepository.Create(client);

            var role = new AspNetRole()
            {
                Name = "Admin",
                ClientId = client.Id,
                IsAdmin = true
            };

            await _roles.CreateAsync(role);

            if (dto.User != null)
            {
                var user = new AspNetUser()
                {
                    UserName = dto.User.Email,
                    Email = dto.User.Email,
                    FirstName = dto.User.FirstName,
                    LastName = dto.User.LastName,
                    ClientId = client.Id
                };

                await _users.CreateAsync(user);
                await _emailService.SendInvitation(user);
            }

            if (dto.Organization != null)
            {
                var organization = Organization.NewOrganization(client.Id);

                organization.Name = dto.Name;

                await _organizationDataService.SaveAsync(organization, _currentUser.UserId);
            }

            return ConvertToDto(client);
        }

        public async Task<ClientDto> Update(int id, UpdateClientDto dto)
        {
            var client = await _clientRepository.GetById(id);

            client.Name = dto.Name;
            client.TradeName = dto.TradeName;
            client.Address = dto.Address;
            client.PhoneNumber = dto.PhoneNumber;
            client.ContactPerson = dto.ContactPerson;

            await _clientRepository.Update(client);

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
