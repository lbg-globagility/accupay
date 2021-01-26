using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans.LoanType
{
    public class LoanTypeService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUser _currentUser;

        public LoanTypeService(
            IProductRepository productRepository,
            ICurrentUser currentUser)
        {
            _productRepository = productRepository;
            _currentUser = currentUser;
        }

        internal async Task<LoanTypeDto> CreateAsync(LoanTypeDto dto)
        {
            var loanType = await _productRepository.AddLoanTypeAsync(dto.Name, _currentUser.OrganizationId, _currentUser.UserId);

            dto.Id = loanType.RowID.Value;

            return dto;
        }

        internal async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
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
            var paginatedListResult = await _productRepository.GetPaginatedLoanTypeListAsync(
                options: options,
                searchTerm: term,
                organizationId: _currentUser.OrganizationId);

            return paginatedListResult.Select(p => LoanTypeDto.Convert(p));
        }
    }
}
