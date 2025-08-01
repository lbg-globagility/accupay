using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Organizations
{
    public class OrganizationService
    {
        private readonly IOrganizationRepository _repository;
        private readonly IOrganizationDataService _dataService;
        private readonly ICurrentUser _currentUser;

        public OrganizationService(
            IOrganizationRepository repository,
            IOrganizationDataService dataService,
            ICurrentUser currentUser)
        {
            _repository = repository;
            _dataService = dataService;
            _currentUser = currentUser;
        }

        public async Task<OrganizationDto> GetById(int organizationId)
        {
            var organization = await _repository.GetByIdAsync(organizationId);
            return ConvertToDto(organization);
        }

        public async Task<OrganizationDto> Create(CreateOrganizationDto dto)
        {
            var organization = Organization.NewOrganization(_currentUser.ClientId);

            organization.Name = dto.Name;

            await _dataService.SaveAsync(organization, _currentUser.UserId);

            return ConvertToDto(organization);
        }

        public async Task<OrganizationDto> Update(int organizationId, UpdateOrganizationDto dto)
        {
            var organization = await _repository.GetByIdAsync(organizationId);
            organization.Name = dto.Name;

            await _dataService.SaveAsync(organization, _currentUser.UserId);

            return ConvertToDto(organization);
        }

        public async Task<PaginatedList<OrganizationDto>> List(OrganizationPageOptions options)
        {
            var (organizations, total) = await _repository.List(options, _currentUser.ClientId);
            var dtos = organizations.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<OrganizationDto>(dtos, total, 1, 1);
        }

        public async Task<ActionResult<OrganizationDto>> GetCurrentOrganization()
        {
            var organization = await _repository.GetByIdAsync(_currentUser.OrganizationId);
            var dto = ConvertToDto(organization);

            return dto;
        }

        public OrganizationDto ConvertToDto(Organization organization)
        {
            var dto = new OrganizationDto()
            {
                Id = organization.RowID,
                Name = organization.Name
            };

            return dto;
        }
    }
}
