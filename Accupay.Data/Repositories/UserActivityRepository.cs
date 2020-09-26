using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
            this._context = context;
        }

        public IEnumerable<UserActivity> GetAll(int? organizationId = null, string entityName = null)
        {
            var query = _context.UserActivities
                .Include(x => x.ActivityItems)
                .Include(x => x.User)
                .AsQueryable();

            if (organizationId != null)
            {
                query = query.Where(x => x.OrganizationID == organizationId);
            }

            if (!string.IsNullOrWhiteSpace(entityName))
            {
                query = query.Where(x => x.EntityName == entityName);
            }

            return query.ToList();
        }

        public void RecordAdd(
            int userId,
            string entityName,
            int entityId,
            int organizationId,
            string suffixIdentifier = "",
            int? changedEmployeeId = null,
            int? changedUserId = null)
        {
            RecordSimple(
                userId,
                entityName,
                "Created a new",
                suffixIdentifier,
                entityId: entityId,
                organizationId: organizationId,
                RecordTypeAdd,
                changedEmployeeId: changedEmployeeId,
                changedUserId: changedUserId);
        }

        public void RecordDelete(
            int userId,
            string entityName,
            int entityId,
            int organizationId,
            string suffixIdentifier = "",
            int? changedEmployeeId = null,
            int? changedUserId = null)
        {
            RecordSimple(
                userId,
                entityName,
                $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}",
                suffixIdentifier,
                entityId: entityId,
                organizationId: organizationId,
                RecordTypeDelete,
                changedEmployeeId: changedEmployeeId,
                changedUserId: changedUserId);
        }

        private void RecordSimple(
            int userId,
            string entityName,
            string simpleDescription,
            string suffixIdentifier,
            int entityId,
            int organizationId,
            string recordType,
            int? changedEmployeeId,
            int? changedUserId)
        {
            entityName = SetStringToPascalCase(entityName);

            var activityItems = new List<UserActivityItem>()
                {
                    new UserActivityItem()
                    {
                        EntityId = entityId,
                        Description = $"{simpleDescription} {entityName?.ToLower()}{suffixIdentifier}.",
                        ChangedEmployeeId = changedEmployeeId,
                        ChangedUserId = changedUserId
                    }
                };

            CreateRecord(userId, entityName, organizationId, recordType, activityItems);
        }

        public void CreateRecord(
            int userId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            CreateUserActivityEntity(userId, entityName, organizationId, recordType, activityItems);
            _context.SaveChanges();
        }

        public async Task CreateRecordAsync(
            int userId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            CreateUserActivityEntity(userId, entityName, organizationId, recordType, activityItems);
            await _context.SaveChangesAsync();
        }

        private void CreateUserActivityEntity(
            int userId,
            string entityName,
            int organizationId,
            string recordType,
            List<UserActivityItem> activityItems = null)
        {
            _context.UserActivities.Add(new UserActivity()
            {
                Created = DateTime.Now,
                UserId = userId,
                EntityName = entityName.ToUpper(),
                ActivityItems = activityItems,
                OrganizationID = organizationId,
                RecordType = recordType
            });
        }

        public static bool CheckIfFirstLetterIsVowel(string entityName)
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
    }
}