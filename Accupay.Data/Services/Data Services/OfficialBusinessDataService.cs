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
    public class OfficialBusinessDataService : BaseSavableDataService<OfficialBusiness>
    {
        private readonly OfficialBusinessRepository _officialBusinessRepository;
        private readonly PayrollContext _context;

        public OfficialBusinessDataService(
            OfficialBusinessRepository officialBusinessRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context) : base(officialBusinessRepository, payPeriodRepository)
        {
            _officialBusinessRepository = officialBusinessRepository;
            _context = context;
        }

        public async Task DeleteAsync(int officialBusinessId)
        {
            var officialBusiness = await _officialBusinessRepository.GetByIdAsync(officialBusinessId);

            if (officialBusiness == null)
                throw new BusinessLogicException("Official Business does not exists.");

            await _officialBusinessRepository.DeleteAsync(officialBusiness);
        }

        protected override async Task SanitizeEntity(OfficialBusiness officialBusiness)
        {
            if (officialBusiness.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (officialBusiness.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (officialBusiness.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (officialBusiness.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (officialBusiness.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (new string[] { OfficialBusiness.StatusPending, OfficialBusiness.StatusApproved }
                            .Contains(officialBusiness.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            var doesExistQuery = _context.OfficialBusinesses
                                    .Where(l => l.EmployeeID == officialBusiness.EmployeeID)
                                    .Where(l => l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date);

            if (IsNewEntity(officialBusiness.RowID) == false)
            {
                doesExistQuery = doesExistQuery.Where(l => officialBusiness.RowID != l.RowID);
            }

            if (await doesExistQuery.AnyAsync())
                throw new BusinessLogicException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            if (officialBusiness.StartTime == officialBusiness.EndTime)
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            officialBusiness.UpdateEndDate();
        }

        #region Queries

        public async Task<OfficialBusiness> GetByIdAsync(int officialBusinessId)
        {
            return await _officialBusinessRepository.GetByIdAsync(officialBusinessId);
        }

        public async Task<OfficialBusiness> GetByIdWithEmployeeAsync(int officialBusinessId)
        {
            return await _officialBusinessRepository.GetByIdWithEmployeeAsync(officialBusinessId);
        }

        public async Task<IEnumerable<OfficialBusiness>> GetByEmployeeAsync(int employeeId)
        {
            return await _officialBusinessRepository.GetByEmployeeAsync(employeeId);
        }

        public async Task<PaginatedList<OfficialBusiness>> GetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            return await _officialBusinessRepository.GetPaginatedListAsync(options, organizationId, searchTerm, dateFrom, dateTo);
        }

        public List<string> GetStatusList()
        {
            return _officialBusinessRepository.GetStatusList();
        }

        #endregion Queries
    }
}