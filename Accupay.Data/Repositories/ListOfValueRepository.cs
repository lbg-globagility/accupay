using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ListOfValueRepository
    {
        public async Task<IEnumerable<ListOfValue>> GetFilteredListOfValues(Expression<Func<ListOfValue, bool>> filter)
        {
            using (var context = new PayrollContext())

            {
                return await context.ListOfValues.
                                        Where(filter).
                                        ToListAsync();
            }
        }

        public async Task<ListOfValue> GetOrCreateOfficialBusinessTypeAsync(string name,
                                                                       int userId,
                                                                       int? organizationId = null)
        {
            return await GetOrCreateListOfValueAsync("Official Business Type",
                                                name,
                                                userId: userId,
                                                organizationId: organizationId);
        }

        public async Task<IEnumerable<ListOfValue>> GetOfficialBusinessTypesAsync()
        {
            return await GetListOfValuesAsync("Official Business Type");
        }

        public IEnumerable<ListOfValue> GetLeaveConvertiblePolicies()
        {
            return GetListOfValues("LeaveConvertiblePolicy");
        }

        public async Task<IEnumerable<ListOfValue>> GetDeductionSchedulesAsync()
        {
            return await GetListOfValuesAsync("Government deduction schedule");
        }

        public async Task<IEnumerable<ListOfValue>> GetDutyShiftPoliciesAsync()
        {
            return await GetListOfValuesAsync("DutyShift");
        }

        public async Task<IEnumerable<ListOfValue>> GetShiftPoliciesAsync()
        {
            return await GetListOfValuesAsync("ShiftPolicy");
        }

        public async Task<IEnumerable<ListOfValue>> GetListOfValuesAsync(string type)
        {
            using (var context = new PayrollContext())
            {
                return await context.ListOfValues.
                                        Where(l => l.Type == type).
                                        Where(l => l.Active == "Yes").
                                        ToListAsync();
            }
        }

        public IEnumerable<ListOfValue> GetListOfValues(string type)
        {
            using (var context = new PayrollContext())
            {
                return context.ListOfValues.
                                Where(l => l.Type == type).
                                Where(l => l.Active == "Yes").
                                ToList();
            }
        }

        private async Task<ListOfValue> GetListOfValueAsync(string type, string name)
        {
            using (var context = new PayrollContext())
            {
                return await context.ListOfValues.
                                        Where(l => l.Type == type).
                                        Where(l => l.Active == "Yes").
                                        Where(l => l.DisplayValue.ToLower() == name.ToLower()).
                                        FirstOrDefaultAsync();
            }
        }

        private async Task<ListOfValue> GetOrCreateListOfValueAsync(string type,
                                                                string name,
                                                                int userId,
                                                                int? organizationId = null)
        {
            var listOfValue = await GetListOfValueAsync(type, name);

            if (listOfValue != null) return listOfValue;

            using (var context = new PayrollContext())
            {
                var listOfVal = new ListOfValue();
                listOfVal.DisplayValue = name.Trim();
                listOfVal.Type = "Official Business Type";
                listOfVal.Active = "Yes";

                listOfVal.Created = DateTime.Now;
                listOfVal.CreatedBy = userId;
                listOfVal.LastUpd = DateTime.Now;
                listOfVal.LastUpdBy = userId;
                listOfVal.OrganizationID = organizationId;

                context.ListOfValues.Add(listOfVal);

                await context.SaveChangesAsync();

                return listOfValue;
            }
        }

        public List<string> ConvertToStringList(IEnumerable<ListOfValue> listOfValues, string columnName = "DisplayValue")
        {
            List<string> stringList;
            stringList = new List<string>();

            foreach (var listOfValue in listOfValues)
            {
                switch (columnName)
                {
                    case "LIC":
                        {
                            stringList.Add(listOfValue.LIC);
                            break;
                        }

                    default:
                        {
                            stringList.Add(listOfValue.DisplayValue);
                            break;
                        }
                }
            }

            return stringList;
        }
    }
}