using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class DivisionDataService
    {
        private readonly DivisionRepository _divisionRepository;
        private readonly ListOfValueRepository _listOfValueRepository;

        public DivisionDataService(DivisionRepository repository, ListOfValueRepository listOfValueRepository)
        {
            _divisionRepository = repository;
            _listOfValueRepository = listOfValueRepository;
        }

        public async Task<PaginatedListResult<Division>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm)
        {
            return await _divisionRepository.GetPaginatedListAsync(options, organizationId, isRoot: false, searchTerm);
        }

        public async Task<Division> GetByIdAsync(int id)
        {
            return await _divisionRepository.GetByIdAsync(id);
        }

        public async Task SaveAsync(Division division, int organizationId)
        {
            await _divisionRepository.SaveAsync(division, organizationId);
        }

        public async Task<IEnumerable<Division>> GettAllParentsAsync(int organizationId)
        {
            return await _divisionRepository.GetAllParentsAsync(organizationId);
        }

        public IEnumerable<string> GetTypes()
        {
            return _divisionRepository.GetDivisionTypeList();
        }

        public async Task<IEnumerable<string>> GetSchedulesAsync()
        {
            IEnumerable<ListOfValue> listOfValues = await _listOfValueRepository.GetDeductionSchedulesAsync();
            return listOfValues.Select(x => x.DisplayValue);
        }
    }
}