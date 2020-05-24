using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
            IQueryable<UserActivity> query = _context.UserActivities.
                                                Include(x => x.ActivityItems).
                                                Include(x => x.User);

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

        public void RecordAdd(int userId, string entityName, int entityId, int organizationId)
        {
            RecordSimple(userId, entityName, "Created a new", entityId, organizationId, RecordTypeAdd);
        }

        public void RecordDelete(int userId, string entityName, int entityId, int organizationId)
        {
            RecordSimple(userId,
                        entityName,
                        $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}",
                        entityId,
                        organizationId,
                        RecordTypeDelete);
        }

        private void RecordSimple(int userId,
                                    string entityName,
                                    string simpleDescription,
                                    int entityId,
                                    int organizationId,
                                    string recordType)
        {
            entityName = SetStringToPascalCase(entityName);

            var activityItems = new List<UserActivityItem>()
                {
                    new UserActivityItem()
                    {
                        EntityId = entityId,
                        Description = $"{simpleDescription} {entityName?.ToLower()}."
                    }
                };

            CreateRecord(userId, entityName, organizationId, recordType, activityItems);
        }

        public void CreateRecord(int userId,
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

            _context.SaveChanges();
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