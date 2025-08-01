using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.EmploymentPolicies.Models;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.EmploymentPolicies.Services
{
    public class EmploymentPolicyService
    {
        private readonly IEmploymentPolicyRepository _repository;

        public EmploymentPolicyService(IEmploymentPolicyRepository repository)
        {
            _repository = repository;
        }

        public async Task<EmploymentPolicyDto> GetById(int employmentPolicyId)
        {
            var employmentPolicy = await _repository.GetById(employmentPolicyId);

            return ConvertToDto(employmentPolicy);
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
            var types = new EmploymentPolicyTypesCollection(await _repository.GetAllTypes());
            var employmentPolicy = await _repository.GetById(employmentPolicyId);
            employmentPolicy.Name = dto.Name;

            employmentPolicy.SetItem(types.Find("WorkDaysPerYear"), dto.WorkDaysPerYear);
            employmentPolicy.SetItem(types.Find("GracePeriod"), dto.GracePeriod);
            employmentPolicy.SetItem(types.Find("ComputeNightDiff"), dto.ComputeNightDiff);
            employmentPolicy.SetItem(types.Find("ComputeNightDiffOT"), dto.ComputeNightDiff);
            employmentPolicy.SetItem(types.Find("ComputeRestDay"), dto.ComputeRestDay);
            employmentPolicy.SetItem(types.Find("ComputeRestDayOT"), dto.ComputeRestDayOT);
            employmentPolicy.SetItem(types.Find("ComputeSpecialHoliday"), dto.ComputeSpecialHoliday);
            employmentPolicy.SetItem(types.Find("ComputeRegularHoliday"), dto.ComputeRegularHoliday);

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
                Name = employmentPolicy.Name,
                WorkDaysPerYear = employmentPolicy.WorkDaysPerYear,
                GracePeriod = employmentPolicy.GracePeriod,
                ComputeNightDiff = employmentPolicy.ComputeNightDiff,
                ComputeNightDiffOT = employmentPolicy.ComputeNightDiffOT,
                ComputeRestDay = employmentPolicy.ComputeRestDay,
                ComputeRestDayOT = employmentPolicy.ComputeRestDayOT,
                ComputeSpecialHoliday = employmentPolicy.ComputeSpecialHoliday,
                ComputeRegularHoliday = employmentPolicy.ComputeRegularHoliday
            };

            return dto;
        }
    }
}
