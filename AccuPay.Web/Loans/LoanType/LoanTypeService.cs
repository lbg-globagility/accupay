using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans.LoanType
{
    public class LoanTypeService
    {
        private readonly ProductRepository _productRepository;
        private readonly ICurrentUser _currentUser;

        public LoanTypeService(ProductRepository productRepository, ICurrentUser currentUser)
        {
            _productRepository = productRepository;
            _currentUser = currentUser;
        }

        internal async Task<LoanTypeDto> CreateAsync(LoanTypeDto dto)
        {
            var loanType = await _productRepository.AddLoanTypeAsync(dto.Name, _currentUser.OrganizationId, _currentUser.DesktopUserId);

            dto.Id = loanType.RowID.Value;

            return dto;
        }

        internal async Task<Data.Entities.Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetLoanTypeByIdAsync(id);
        }

        internal async Task UpdateAsync(int id, LoanTypeDto dto)
        {
            await _productRepository.UpdateLoanTypeAsync(id, dto.Name);
        }

        internal async Task DeleteAsync(int id)
        {
            await _productRepository.DeleteLoanTypeAsync(id);
        }

        internal async Task<PaginatedList<LoanTypeDto>> GetPaginatedListAsync(PageOptions options, string term)
        {
            var paginatedListResult = await _productRepository.GetPaginatedLoanTypeListAsync(options: options, searchTerm: term, organizationId: _currentUser.OrganizationId);

            var fsdfs11 = paginatedListResult.List;
            var convertedList = fsdfs11.Select(p => LoanTypeDto.Convert(p)).ToList();

            var count = paginatedListResult.TotalCount;

            return new PaginatedList<LoanTypeDto>(convertedList, count, ++options.PageIndex, options.PageSize);
        }
    }
}
