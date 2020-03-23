using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;
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

        public void RecordAdd(int userId, string entityName)
        {
            entityName = SetFirstLetterLowerCase(entityName);

            RecordSimple(userId, entityName, "Created a new");
        }

        public void RecordDelete(int userId, string entityName)
        {
            RecordSimple(userId, entityName,
                $"Deleted {(CheckIfFirstLetterIsVowel(entityName) ? "an" : "a")}");
        }

        private void RecordSimple(int userId, string entityName, string simpleDescription)
        {
            entityName = SetFirstLetterLowerCase(entityName);

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

        private static string SetFirstLetterLowerCase(string entityName)
        {
            if (!string.IsNullOrWhiteSpace(entityName))
            {
                entityName = entityName[0].ToString().ToLower() +
                    (entityName.Count() > 1 ? entityName.Substring(1) : "");
            }

            return entityName;
        }
    }
}