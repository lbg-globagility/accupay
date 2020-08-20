using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Overtimes;
using AccuPay.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OvertimeDataService : BaseDailyPayrollDataService<Overtime>
    {
        private readonly OvertimeRepository _overtimeRepository;

        public OvertimeDataService(
            OvertimeRepository overtimeRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(overtimeRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Overtime")
        {
            _overtimeRepository = overtimeRepository;
        }

        public async Task DeleteManyAsync(IEnumerable<int> overtimeIds)
        {
            await _overtimeRepository.DeleteManyAsync(overtimeIds);
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        protected override async Task SanitizeEntity(Overtime overtime, Overtime oldOvertime)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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
        }

        public async Task<List<Overtime>> BatchApply(IReadOnlyCollection<OvertimeImportModel> validRecords, int organizationId, int userId)
        {
            List<Overtime> overtimes = new List<Overtime>();

            foreach (var ot in validRecords)
            {
                overtimes.Add(new Overtime()
                {
                    CreatedBy = userId,
                    EmployeeID = ot.EmployeeID,
                    OrganizationID = organizationId,
                    OTEndTimeFull = ot.EndTime.Value,
                    OTStartDate = ot.StartDate.Value,
                    OTStartTimeFull = ot.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await _overtimeRepository.SaveManyAsync(overtimes);

            return overtimes;
        }
    }
}