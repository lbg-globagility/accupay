using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class DivisionDataService
    {
        private readonly DivisionRepository _repository;

        public DivisionDataService(DivisionRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedListResult<Division>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm)
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, isRoot: false, searchTerm);
        }
    }
}