using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using System.Threading.Tasks;

namespace AccuPay.Web.Appraisers
{
    public class ApproverService
    {
        private readonly IApproverRepository _repository;
        private readonly ICurrentUser _currentUser;

        public ApproverService(IApproverRepository repository, ICurrentUser currentUser)
        {
            _repository = repository;
            _currentUser = currentUser;
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
                CompanyName = approver.CompanyName
            };
        }
    }
}
