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
    public class OvertimeDataService
    {
        private readonly OvertimeRepository _repository;

        public OvertimeDataService(OvertimeRepository repository)
        {
            _repository = repository;
        }

        public async Task DeleteAsync(int ovetimeId)
        {
            var overtime = await _repository.GetByIdAsync(ovetimeId);

            if (overtime == null)
                throw new BusinessLogicException("Overtime does not exists.");

            await _repository.DeleteAsync(overtime);
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds)
        {
            await _repository.DeleteManyAsync(overtimeIds);
        }

        public async Task SaveAsync(Overtime overtime)
        {
            SanitizeEntity(overtime);

            await _repository.SaveAsync(overtime);
        }

        public async Task SaveManyAsync(List<Overtime> overtimes)
        {
            overtimes.ForEach(x => SanitizeEntity(x));

            await _repository.SaveManyAsync(overtimes);
        }

        private static void SanitizeEntity(Overtime overtime)
        {
            if (overtime.OTStartTime == null)
                throw new BusinessLogicException("Start Time cannot be empty.");

            if (overtime.OTEndTime == null)
                throw new BusinessLogicException("End Time cannot be empty.");

            string[] invalidStatuses = { Overtime.StatusPending, Overtime.StatusApproved };
            if (!invalidStatuses.Contains(overtime.Status))
                throw new BusinessLogicException("Status is not valid.");

            overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds();
            overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds();
            overtime.UpdateEndDate();
        }

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

        public async Task<PaginatedListResult<Overtime>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            return await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);
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
    }
}