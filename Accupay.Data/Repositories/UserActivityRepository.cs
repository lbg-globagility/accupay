using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Data.Entities.UserActivity;

namespace AccuPay.Data.Repositories
{
    public class UserActivityRepository
    {
        public const string RecordTypeAdd = "ADD";
        public const string RecordTypeEdit = "EDIT";
        public const string RecordTypeDelete = "DELETE";
        public const string RecordTypeImport = "IMPORT";

        private readonly PayrollContext _context;

        public UserActivityRepository(PayrollContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<UserActivityItem>> GetPaginatedListAsync(
            PageOptions options,
            int? organizationId = null,
            string entityName = null,
            int? changedByUserId = null,
            ChangedType? changedType = null,
            int? changedEntityId = null,
            string description = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null)
        {
            var query = _context.UserActivityItems
                .Include(x => x.ChangedEmployee)
                .Include(x => x.ChangedUser)
                .Include(x => x.Activity)
                    .ThenInclude(x => x.User)
                .AsQueryable();

            if (organizationId != null)
            {
                query = query.Where(x => x.Activity.OrganizationID == organizationId);
            }

            if (!string.IsNullOrWhiteSpace(entityName))
            {
                query = query.Where(x => x.Activity.EntityName == entityName);
            }

            if (!string.IsNullOrWhiteSpace(options.Sort))
            {
                if (options.Sort == "Created")
                {
                    query = query.OrderBy(x => x.Created, options.Direction);
                }
            }

            if (changedByUserId != null)
            {
                query = query.Where(x => x.Activity.UserId == changedByUserId);
            }

            if (changedEntityId != null)
            {
                if (changedType == ChangedType.User)
                {
                    query = query.Where(x => x.ChangedUserId == changedEntityId);
                }
                else if (changedType == ChangedType.Employee)
                {
                    query = query.Where(x => x.ChangedEmployeeId == changedEntityId);
                }
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                var searchTerm = $"%{description}%";

                query = query.Where(x => EF.Functions.Like(x.Description, searchTerm));
            }

            if (dateFrom != null)
            {
                dateFrom = dateFrom.ToMinimumHourValue();
                query = query.Where(x => x.Created >= dateFrom);
            }

            if (dateTo != null)
            {
                dateTo = dateTo.ToMaximumHourValue();
                query = query.Where(x => x.Created <= dateTo);
            }

            var userActivities = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<UserActivityItem>(userActivities, count, options.PageSize);
        }

        public async Task RecordAddAsync(
            int currentlyLoggedInUserId,
            string entityName,
            int entityId,
            int organizationId,
            string suffixIdentifier = "",
            int? changedEmployeeId = null,
            int? changedUserId = null)
        {
            await RecordSimpleAsync(
                currentlyLoggedInUserId,
                entityName,
                "Created a new",
                suffixIdentifier,
                entityId: entityId,
                organizationId: organizationId,
                RecordTypeAdd,
                changedEmployeeId: changedEmployeeId,
                changedByUserId: changedUserId);
        }

        public async Task RecordDeleteAsync(
            int currentlyLoggedInUserId,
            string entityName,
            int entityId,
            int organizationId,
            string suffixIdentifier = "",
            int? changedEmployeeId = null,
            int? changedUserId = null)
        {
            await RecordSimpleAsync(
                currentlyLoggedInUserId,
                entityName,
                $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}",
                suffixIdentifier,
                entityId: entityId,
                organizationId: organizationId,
                RecordTypeDelete,
                changedEmployeeId: changedEmployeeId,
                changedByUserId: changedUserId);
        }

        public void CreateRecord(
            int currentlyLoggedInUserId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            CreateUserActivityEntity(currentlyLoggedInUserId, entityName, organizationId, recordType, activityItems);
            _context.SaveChanges();
        }

        public async Task CreateRecordAsync(
            int currentlyLoggedInUserId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            CreateUserActivityEntity(currentlyLoggedInUserId, entityName, organizationId, recordType, activityItems);
            await _context.SaveChangesAsync();
        }

        #region Private Methods

        private void CreateUserActivityEntity(
            int currentlyLoggedInUserId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            _context.UserActivities.Add(new UserActivity()
            {
                Created = DateTime.Now,
                UserId = currentlyLoggedInUserId,
                EntityName = entityName.ToUpper(),
                ActivityItems = activityItems,
                OrganizationID = organizationId,
                RecordType = recordType
            });
        }

        private async Task RecordSimpleAsync(
            int currentlyLoggedInUserId,
            string entityName,
            string simpleDescription,
            string suffixIdentifier,
            int entityId,
            int organizationId,
            string recordType,
            int? changedEmployeeId,
            int? changedByUserId)
        {
            entityName = SetStringToPascalCase(entityName);

            List<UserActivityItem> activityItems = CreateSimpleUserActivityItem(
                entityName: entityName,
                simpleDescription: simpleDescription,
                suffixIdentifier: suffixIdentifier,
                entityId: entityId,
                changedEmployeeId: changedEmployeeId,
                changedByUserId: changedByUserId);

            await CreateRecordAsync(currentlyLoggedInUserId, entityName, organizationId, recordType, activityItems);
        }

        private static List<UserActivityItem> CreateSimpleUserActivityItem(
            string entityName,
            string simpleDescription,
            string suffixIdentifier,
            int entityId,
            int? changedEmployeeId,
            int? changedByUserId)
        {
            return new List<UserActivityItem>()
            {
                new UserActivityItem()
                {
                    EntityId = entityId,
                    Description = $"{simpleDescription} {entityName?.ToLower()}{suffixIdentifier}.",
                    ChangedEmployeeId = changedEmployeeId,
                    ChangedUserId = changedByUserId
                }
            };
        }

        private static bool CheckIfFirstLetterIsVowel(string entityName)
        {
            if (!string.IsNullOrWhiteSpace(entityName))
            {
                return "aeiouAEIOU".IndexOf(entityName[0]) >= 0;
            }

            return false;
        }

        private static string SetStringToPascalCase(string entityName)
        {
            if (!string.IsNullOrWhiteSpace(entityName))
            {
                var textInfo = new CultureInfo("en-US", false).TextInfo;
                entityName = textInfo.ToTitleCase(entityName);
            }

            return entityName;
        }

        #endregion Private Methods
    }
}
