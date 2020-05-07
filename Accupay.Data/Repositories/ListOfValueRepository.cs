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
        public IEnumerable<ListOfValue> GetLeaveConvertiblePolicies()
        {
            return GetListOfValues("LeaveConvertiblePolicy");
        }

        public async Task<IEnumerable<ListOfValue>> GetFilteredListOfValuesAsync(Expression<Func<ListOfValue, bool>> filter)
        {
            using (var context = new PayrollContext())

            {
                return await context.ListOfValues.
                                        Where(filter).
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<ListOfValue>> GetDeductionSchedulesAsync()
        {
            return await GetListOfValuesAsync("Government deduction schedule");
        }

        public async Task<IEnumerable<ListOfValue>> GetEmployeeChecklistsAsync()
        {
            return await GetListOfValuesAsync("Employee Checklist");
        }

        public async Task<IEnumerable<ListOfValue>> GetEmployeeDisciplinaryPenalties()
        {
            return await GetListOfValuesAsync("Employee Disciplinary Penalty");
        }

        public async Task<IEnumerable<ListOfValue>> GetDutyShiftPoliciesAsync()
        {
            return await GetListOfValuesAsync("DutyShift");
        }

        public async Task<IEnumerable<ListOfValue>> GetShiftPoliciesAsync()
        {
            return await GetListOfValuesAsync("ShiftPolicy");
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

        public async Task DeleteAsync(ListOfValue value)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.ListOfValues.Remove(value);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(ListOfValue value)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.Entry(value).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateAsync(ListOfValue value)
        {
            using (PayrollContext context = new PayrollContext())
            {
                context.ListOfValues.Add(value);
                await context.SaveChangesAsync();
            }
        }
    }
}