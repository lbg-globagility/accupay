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

        public IEnumerable<UserActivity> ListWithItems(string entityName = null)
        {
            using (PayrollContext context = new PayrollContext())
            {
                IQueryable<UserActivity> query = context.UserActivities.
                                                    Include(x => x.ActivityItems);

                if (!string.IsNullOrWhiteSpace(entityName))
                {
                    query = query.Where(x => x.EntityName == entityName);
                }

                return query.ToList();
            }
        }

        public void RecordAdd(int userId, string entityName)
        {
            RecordSimple(userId, entityName, "Created a new");
        }

        public void RecordDelete(int userId, string entityName)
        {
            RecordSimple(userId, entityName,
                $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}");
        }

        private void RecordSimple(int userId, string entityName, string simpleDescription)
        {
            entityName = SetStringToPascalCase(entityName);

            var acitivityItems = new List<UserActivityItem>()
                {
                    new UserActivityItem()
                    {
                        Description = $"{simpleDescription} {entityName}."
                    }
                };

            AddItem(userId, entityName, acitivityItems);
        }

        public void AddItem(int userId, string entityName, List<UserActivityItem> acitivityItems = null)
        {
            using (var context = new PayrollContext())
            {
                context.UserActivities.Add(new UserActivity()
                {
                    Created = DateTime.Now,
                    UserId = userId,
                    EntityName = entityName.ToUpper(),

                    ActivityItems = acitivityItems
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