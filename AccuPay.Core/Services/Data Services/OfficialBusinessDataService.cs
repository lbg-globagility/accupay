using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Repositories;
using AccuPay.Core.Services.Imports.OfficialBusiness;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class OfficialBusinessDataService : BaseDailyPayrollDataService<OfficialBusiness>
    {
        private const string UserActivityName = "Official Business";

        public OfficialBusinessDataService(
            OfficialBusinessRepository officialBusinessRepository,
            PayPeriodRepository payPeriodRepository,
            UserActivityRepository userActivityRepository,
            PayrollContext context,
            IPolicyHelper policy) :

            base(officialBusinessRepository,
                payPeriodRepository,
                userActivityRepository,
                context,
                policy,
                entityName: "Official Business",
                entityNamePlural: "Official Businesses")
        {
        }

        public async Task<List<OfficialBusiness>> BatchApply(IReadOnlyCollection<OfficialBusinessImportModel> validRecords, int organizationId, int currentlyLoggedInUserId)
        {
            List<OfficialBusiness> officialBusinesses = new List<OfficialBusiness>();

            foreach (var ob in validRecords)
            {
                officialBusinesses.Add(new OfficialBusiness()
                {
                    EmployeeID = ob.EmployeeID,
                    OrganizationID = organizationId,
                    EndTimeFull = ob.EndTime.Value,
                    StartDate = ob.StartDate.Value,
                    StartTimeFull = ob.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await SaveManyAsync(officialBusinesses, currentlyLoggedInUserId);

            return officialBusinesses;
        }

        #region Overrides

        protected override string GetUserActivityName(OfficialBusiness officialBusiness)
        {
            return UserActivityName;
        }

        protected override string CreateUserActivitySuffixIdentifier(OfficialBusiness officialBusiness)
        {
            return $" with date '{officialBusiness.StartDate.ToShortDateString()}'";
        }

        protected override async Task SanitizeEntity(OfficialBusiness officialBusiness, OfficialBusiness oldOfficialBusiness, int changedByUserId)
        {
            await base.SanitizeEntity(
                entity: officialBusiness,
                oldEntity: oldOfficialBusiness,
                currentlyLoggedInUserId: changedByUserId);

            if (officialBusiness.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (officialBusiness.StartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753.");

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

            if (officialBusiness.IsNewEntity == false)
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

        protected override async Task RecordUpdate(OfficialBusiness newValue, OfficialBusiness oldValue)
        {
            var changes = new List<UserActivityItem>();
            var entityName = UserActivityName.ToLower();

            var suffixIdentifier = $"of {entityName}{CreateUserActivitySuffixIdentifier(oldValue)}.";

            if (newValue.StartDate != oldValue.StartDate)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start date from '{oldValue.StartDate?.ToShortDateString()}' to '{newValue.StartDate?.ToShortDateString()}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.StartTime != oldValue.StartTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated start time from '{oldValue.StartTime?.ToStringFormat("hh:mm tt")}' to '{newValue.StartTime?.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.EndTime != oldValue.EndTime)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated end time from '{oldValue.EndTime?.ToStringFormat("hh:mm tt")}' to '{newValue.EndTime?.ToStringFormat("hh:mm tt")}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Reason != oldValue.Reason)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated reason from '{oldValue.Reason}' to '{newValue.Reason}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Comments != oldValue.Comments)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated comments from '{oldValue.Comments}' to '{newValue.Comments}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });
            if (newValue.Status != oldValue.Status)
                changes.Add(new UserActivityItem()
                {
                    EntityId = oldValue.RowID.Value,
                    Description = $"Updated status from '{oldValue.Status}' to '{newValue.Status}' {suffixIdentifier}",
                    ChangedEmployeeId = oldValue.EmployeeID.Value
                });

            if (changes.Any())
            {
                await _userActivityRepository.CreateRecordAsync(
                    newValue.LastUpdBy.Value,
                    UserActivityName,
                    newValue.OrganizationID.Value,
                    UserActivityRepository.RecordTypeEdit,
                    changes);
            }
        }

        #endregion Overrides
    }
}
