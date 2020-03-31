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
        public IEnumerable<UserActivity> List
        {
            get
            {
                using (PayrollContext context = new PayrollContext())
                {
                    return context.UserActivities.ToList();
                }
            }
        }

        public IEnumerable<UserActivity> ListWithItems(int organizationId, string entityName = null)
        {
            using (PayrollContext context = new PayrollContext())
            {
                IQueryable<UserActivity> query = context.UserActivities.
                                                    Include(x => x.ActivityItems).Include(x => x.User)
                                                    .Where(x => x.OrganizationID == organizationId);

                if (!string.IsNullOrWhiteSpace(entityName))
                {
                    query = query.Where(x => x.EntityName == entityName);
                }

                return query.ToList();
            }
        }

        public void RecordAdd(int userId, string entityName, int EntityId, int organizationId)
        {
            RecordSimple(userId, entityName, "Created a new", EntityId, organizationId);
        }

        public void RecordDelete(int userId, string entityName, int EntityId, int organizationId)
        {
            RecordSimple(userId, entityName,
                $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}", EntityId, organizationId);
        }

        private void RecordSimple(int userId, string entityName, string simpleDescription, int EntityId, int organizationId)
        {
            entityName = SetStringToPascalCase(entityName);

            var acitivityItems = new List<UserActivityItem>()
                {
                    new UserActivityItem()
                    {
                        Description = $"{simpleDescription} {entityName}."
                    }
                };

            AddItem(userId, entityName, EntityId, organizationId, acitivityItems);
        }

        public void AddItem(int userId, string entityName, int entiryId, int organizationId, List<UserActivityItem> acitivityItems = null)
        {
            using (var context = new PayrollContext())
            {
                context.UserActivities.Add(new UserActivity()
                {
                    Created = DateTime.Now,
                    UserId = userId,
                    EntityName = entityName.ToUpper(),
                    EntityId = entiryId,
                    ActivityItems = acitivityItems,
                    OrganizationID = organizationId
                });

                context.SaveChanges();
            }
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