using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
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

        public async Task<OrganizationDto> Create(CreateOrganizationDto dto)
        {
            var organization = new Organization()
            {
                Name = dto.Name
            };

            await _repository.Create(organization);

            var createdDto = new OrganizationDto()
            {
                Id = organization.RowID,
                Name = organization.Name
            };

            return createdDto;
        }

        public void Update(UpdateOrganizationDto dto)
        {
        }
    }
}
