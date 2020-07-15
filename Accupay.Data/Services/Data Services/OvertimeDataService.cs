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
    public class OvertimeDataService : BaseSavableDataService<Overtime>
    {
        private readonly OvertimeRepository _overtimeRepository;

        public OvertimeDataService(OvertimeRepository overtimeRepository, PayPeriodRepository payPeriodRepository) : base(overtimeRepository, payPeriodRepository)
        {
            _overtimeRepository = overtimeRepository;
        }

        public async Task DeleteAsync(int overtimeId)
        {
            var overtime = await _overtimeRepository.GetByIdAsync(overtimeId);

            if (overtime == null)
                throw new BusinessLogicException("Overtime does not exists.");

            await _overtimeRepository.DeleteAsync(overtime);
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds)
        {
            await _overtimeRepository.DeleteManyAsync(overtimeIds);
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
            return await _overtimeRepository.GetByIdAsync(overtimeId);
        }

        public async Task<Overtime> GetByIdWithEmployeeAsync(int overtimeId)
        {
            return await _overtimeRepository.GetByIdWithEmployeeAsync(overtimeId);
        }

        public async Task<IEnumerable<Overtime>> GetByEmployeeAsync(int employeeId)
        {
            return await _overtimeRepository.GetByEmployeeAsync(employeeId);
        }

        public async Task<PaginatedList<Overtime>> GetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            string searchTerm = "",
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            return await _overtimeRepository.GetPaginatedListAsync(options, organizationId, searchTerm, dateFrom, dateTo);
        }

        public IEnumerable<Overtime> GetByEmployeeIDsAndDatePeriod(int organizationId,
                                                                List<int> employeeIdList,
                                                                TimePeriod timePeriod,
                                                                OvertimeStatus overtimeStatus = OvertimeStatus.All)
        {
            return _overtimeRepository.GetByEmployeeIDsAndDatePeriod(organizationId,
                                                            employeeIdList,
                                                            timePeriod,
                                                            overtimeStatus);
        }

        public List<string> GetStatusList()
        {
            return _overtimeRepository.GetStatusList();
        }

        #endregion Queries
    }
}