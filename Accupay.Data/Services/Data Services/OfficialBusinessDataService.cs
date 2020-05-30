using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OfficialBusinessDataService : BaseDataService
    {
        private readonly OfficialBusinessRepository _repository;
        private readonly PayrollContext _context;

        public OfficialBusinessDataService(OfficialBusinessRepository repository, PayrollContext context)
        {
            _repository = repository;
            _context = context;
        }

        #region CRUD

        public async Task DeleteAsync(int officialBusinessId)
        {
            var officialBusiness = await _repository.GetByIdAsync(officialBusinessId);

            if (officialBusiness == null)
                throw new BusinessLogicException("Official Business does not exists.");

            await _repository.DeleteAsync(officialBusiness);
        }

        public async Task SaveAsync(OfficialBusiness officialBusiness)
        {
            await SanitizeEntity(officialBusiness);

            await _repository.SaveAsync(officialBusiness);
        }

        public async Task SaveManyAsync(List<OfficialBusiness> officialBusinesses)
        {
            officialBusinesses.ForEach(async (x) => await SanitizeEntity(x));

            await _repository.SaveManyAsync(officialBusinesses);
        }

        private async Task SanitizeEntity(OfficialBusiness officialBusiness)
        {
            if (officialBusiness.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (officialBusiness.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (officialBusiness.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (officialBusiness.EmployeeID == null)
                throw new BusinessLogicException("Employee does not exists.");

            string[] invalidStatuses = { OfficialBusiness.StatusPending, OfficialBusiness.StatusApproved };
            if (!invalidStatuses.Contains(officialBusiness.Status) == false)
                throw new BusinessLogicException("Status is not valid.");

            var doesExistQuery = _context.OfficialBusinesses
                                    .Where(l => l.EmployeeID == officialBusiness.EmployeeID)
                                    .Where(l => l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date);

            if (isNewEntity(officialBusiness.RowID) == false)
            {
                doesExistQuery = doesExistQuery.Where(l => officialBusiness.RowID != l.RowID);
            }

            if (await doesExistQuery.AnyAsync())
                throw new BusinessLogicException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            officialBusiness.UpdateEndDate();
        }

        #endregion CRUD

        public async Task<OfficialBusiness> GetByIdAsync(int officialBusinessId)
        {
            return await _repository.GetByIdAsync(officialBusinessId);
        }

        public async Task<OfficialBusiness> GetByIdWithEmployeeAsync(int officialBusinessId)
        {
            return await _repository.GetByIdWithEmployeeAsync(officialBusinessId);
        }

        public async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            return await _repository.GetByEmployeeAsync(employeeId);
        }

        public async Task<PaginatedListResult<OfficialBusiness>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);
        }

        public List<string> GetStatusList()
        {
            return _repository.GetStatusList();
        }
    }
}