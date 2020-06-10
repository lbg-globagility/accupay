using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.ValueObjects;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OvertimeDataService : BaseDataService<Overtime>
    {
        private readonly OvertimeRepository _repository;

        public OvertimeDataService(OvertimeRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public async Task DeleteAsync(int overtimeId)
        {
            var overtime = await _repository.GetByIdAsync(overtimeId);

            if (overtime == null)
                throw new BusinessLogicException("Overtime does not exists.");

            await _repository.DeleteAsync(overtime);
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds)
        {
            await _repository.DeleteManyAsync(overtimeIds);
        }

        protected override async Task SanitizeEntity(Overtime overtime)
        {
            await Task.Run(() =>
            {
                if (overtime.OrganizationID == null)
                    throw new BusinessLogicException("Organization is required.");

                if (overtime.EmployeeID == null)
                    throw new BusinessLogicException("Employee is required.");

                if (overtime.OTStartTime == null)
                    throw new BusinessLogicException("Start Time is required.");

                if (overtime.OTEndTime == null)
                    throw new BusinessLogicException("End Time is required.");

                if (new string[] { Overtime.StatusPending, Overtime.StatusApproved }
                                .Contains(overtime.Status) == false)
                {
                    throw new BusinessLogicException("Status is not valid.");
                }

                overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();
                overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();

                if (overtime.OTStartTime == overtime.OTEndTime)
                    throw new BusinessLogicException("End Time cannot be equal to Start Time");

                overtime.UpdateEndDate();
            });
        }

        #region Queries

        public async Task<Overtime> GetByIdAsync(int overtimeId)
        {
            return await _repository.GetByIdAsync(overtimeId);
        }

        public async Task<Overtime> GetByIdWithEmployeeAsync(int overtimeId)
        {
            return await _repository.GetByIdWithEmployeeAsync(overtimeId);
        }

        public async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            return await _repository.GetByEmployeeAsync(employeeId);
        }

        public async Task<PaginatedListResult<Overtime>> GetPaginatedListAsync(PageOptions options,
                                                                               int organizationId,
                                                                               string searchTerm = "",
                                                                               DateTime? dateFrom = null,
                                                                               DateTime? dateTo = null)
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, searchTerm, dateFrom, dateTo);
        }

        public IEnumerable<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId,
                                                                TimePeriod timePeriod,
                                                                List<int> employeeIdList,
                                                                OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return _repository.GetByEmployeeIDsAndDatePeriod(organizationId,
                                                            timePeriod,
                                                            employeeIdList,
                                                            overtimeStatus);
        }

        public List<string> GetStatusList()
        {
            return _repository.GetStatusList();
        }

        #endregion Queries
    }
}
