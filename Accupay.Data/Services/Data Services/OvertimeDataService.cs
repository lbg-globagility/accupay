using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OvertimeDataService : BaseSavablePayrollDataService<Overtime>
    {
        private readonly OvertimeRepository _overtimeRepository;

        public OvertimeDataService(
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            PolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                policy,
                entityDoesNotExistOnDeleteErrorMessage: "Overtime does not exists.")
        {
            _overtimeRepository = overtimeRepository;
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

                if (overtime.OTStartDate < PayrollTools.SqlServerMinimumDate)
                    throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

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
    }
}