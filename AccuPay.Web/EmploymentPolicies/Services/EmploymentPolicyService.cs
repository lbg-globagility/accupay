using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.EmploymentPolicies.Models;
using System.Linq;
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
            var types = new EmploymentPolicyTypesCollection(await _repository.GetAllTypes());

            var employmentPolicy = new EmploymentPolicy(dto.Name);

            employmentPolicy.SetItem(types.Find("WorkDaysPerYear"), dto.WorkDaysPerYear);
            employmentPolicy.SetItem(types.Find("GracePeriod"), dto.GracePeriod);
            employmentPolicy.SetItem(types.Find("ComputeNightDiff"), dto.ComputeNightDiff);
            employmentPolicy.SetItem(types.Find("ComputeNightDiffOT"), dto.ComputeNightDiff);
            employmentPolicy.SetItem(types.Find("ComputeRestDay"), dto.ComputeRestDay);
            employmentPolicy.SetItem(types.Find("ComputeRestDayOT"), dto.ComputeRestDayOT);
            employmentPolicy.SetItem(types.Find("ComputeSpecialHoliday"), dto.ComputeSpecialHoliday);
            employmentPolicy.SetItem(types.Find("ComputeRegularHoliday"), dto.ComputeRegularHoliday);

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

        public async Task<PaginatedList<EmploymentPolicyDto>> List(PageOptions options)
        {
            var (employmentPolicies, total) = await _repository.List(options);
            var dtos = employmentPolicies.Select(t => ConvertToDto(t));

            return new PaginatedList<EmploymentPolicyDto>(dtos, total, 1, 1);
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
