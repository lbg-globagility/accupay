using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Organizations
{
    public class OrganizationService
    {
        private readonly OrganizationRepository _repository;

        public OrganizationService(OrganizationRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrganizationDto> GetById(int organizationId)
        {
            var organization = await _repository.GetByIdAsync(organizationId);
            return ConvertToDto(organization);
        }

        public async Task<OrganizationDto> Create(CreateOrganizationDto dto)
        {
            var organization = new Organization()
            {
                Name = dto.Name
            };

            await _repository.Create(organization);

            return ConvertToDto(organization);
        }

        public async Task<OrganizationDto> Update(int organizationId, UpdateOrganizationDto dto)
        {
            var organization = await _repository.GetByIdAsync(organizationId);
            organization.Name = dto.Name;

            await _repository.Update(organization);

            return ConvertToDto(organization);
        }

        public async Task<PaginatedList<OrganizationDto>> List(PageOptions options)
        {
            var (organizations, total) = await _repository.List(options);
            var dtos = organizations.Select(t => ConvertToDto(t)).ToList();

            return new PaginatedList<OrganizationDto>(dtos, total, 1, 1);
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
