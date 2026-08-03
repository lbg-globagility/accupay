using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Web.Appraisers.ApproverDto;

namespace AccuPay.Web.Appraisers
{
    public class ApproverService
    {
        private readonly IApproverRepository _repository;
        private readonly ICurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly IEmployeeApproverRepository _employeeApproverRepository;
        public ApproverService(
            IApproverRepository repository,
            ICurrentUser currentUser,
            IMapper mapper,
            IEmployeeApproverRepository employeeApproverRepository)
        {
            _repository = repository;
            _currentUser = currentUser;
            _mapper = mapper;
            _employeeApproverRepository = employeeApproverRepository;
        }

        public async Task<PaginatedList<ApproverDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            var paginated = await _repository.GetPaginatedListAsync(options, _currentUser.OrganizationId, searchTerm);

            return paginated.Select(a => ConvertToDto(a));
        }

        public async Task<ApproverDto> GetById(int id)
        {
            var approver = await _repository.GetByIdWithOrganizationAsync(id);

            return ConvertToDto(approver);
        }

        public async Task<ApproverDto> Create(CreateApproverDto dto)
        {
            var approver = new Approver()
            {
                OrganizationID = _currentUser.OrganizationId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
                CompanyName = dto.CompanyName,
                IsActive = true
            };

            await _repository.SaveAsync(approver);

            return ConvertToDto(approver);
        }

        public async Task<ApproverDto> Update(int id, UpdateApproverDto dto)
        {
            var approver = await _repository.GetByIdAsync(id);
            if (approver == null) return null;

            approver.FirstName = dto.FirstName;
            approver.LastName = dto.LastName;
            approver.EmailAddress = dto.EmailAddress;
            approver.CompanyName = dto.CompanyName;

            await _repository.SaveAsync(approver);

            return ConvertToDto(approver);
        }

        public async Task SetAsInactive(int id)
        {
            var approver = await _repository.GetByIdAsync(id);
            if (approver == null) return;

            approver.IsActive = false;

            await _repository.SaveAsync(approver);
        }

        private static ApproverDto ConvertToDto(Approver approver)
        {
            if (approver == null) return null;

            return new ApproverDto()
            {
                Id = approver.RowID.Value,
                OrganizationId = approver.OrganizationID,
                OrganizationName = approver.Organization?.Name,
                FirstName = approver.FirstName,
                LastName = approver.LastName,
                EmailAddress = approver.EmailAddress,
                CompanyName = approver.CompanyName,
               

            };
        }
        public async Task<ApproverDto> ApproverEmployees(int id)
        {
            var approver = await _repository.ApproverEmployees(id);
            return _mapper.Map<ApproverDto>(approver);

        }

        public async Task<List<SelfServiceApproverDto>> GetByEmployeeId(int employeeId)
        {
            var employeeApprovers = await _employeeApproverRepository.GetByEmployeeIdAsync(employeeId);
            var active = employeeApprovers
                .Where(ea => ea.Approver != null && ea.Approver.IsActive);
            return active.Select(x => _mapper.Map<SelfServiceApproverDto>(x.Approver)).ToList();
        }
    }
}
