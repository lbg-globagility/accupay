using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.EmploymentPolicies.Models;
using System.Threading.Tasks;

namespace AccuPay.Web.EmploymentPolicies.Services
{
    public class EmploymentPolicyService
    {
        private readonly EmploymentPolicyRepository _repository;

        public EmploymentPolicyService(EmploymentPolicyRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmploymentPolicyDto> Create(CreateEmploymentPolicyDto dto)
        {
            var employmentPolicy = new EmploymentPolicy();
            employmentPolicy.Name = dto.Name;

            await _repository.Create(employmentPolicy);

            return ConvertToDto(employmentPolicy);
        }

        public async Task<EmploymentPolicyDto> Update(int employmentPolicyId, UpdateEmploymentPolicyDto dto)
        {
            var employmentPolicy = await _repository.GetById(employmentPolicyId);
            employmentPolicy.Name = dto.Name;

            await _repository.Update(employmentPolicy);

            return ConvertToDto(employmentPolicy);
        }

        public async Task GetAll()
        {
        }

        private EmploymentPolicyDto ConvertToDto(EmploymentPolicy employmentPolicy)
        {
            var dto = new EmploymentPolicyDto()
            {
                Id = employmentPolicy.Id,
                Name = employmentPolicy.Name
            };

            return dto;
        }
    }
}
