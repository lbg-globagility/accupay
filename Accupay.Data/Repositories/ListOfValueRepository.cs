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
        private readonly PayrollContext _context;

        public ListOfValueRepository(PayrollContext context)
        {
            _context = context;
        }

        public ICollection<ListOfValue> GetLeaveConvertiblePolicies()
        {
            return GetListOfValues("LeaveConvertiblePolicy");
        }

        public async Task<ICollection<ListOfValue>> GetFilteredListOfValuesAsync(Expression<Func<ListOfValue, bool>> filter)
        {
            return await _context.ListOfValues
                .Where(filter)
                .ToListAsync();
        }

        public async Task<ICollection<ListOfValue>> GetDeductionSchedulesAsync()
        {
            return await GetListOfValuesAsync("Government deduction schedule");
        }

        public async Task<ICollection<ListOfValue>> GetEmployeeChecklistsAsync()
        {
            return await GetListOfValuesAsync("Employee Checklist");
        }

        public async Task<ICollection<ListOfValue>> GetEmployeeDisciplinaryPenalties()
        {
            return await GetListOfValuesAsync("Employee Disciplinary Penalty");
        }

        public async Task<ICollection<ListOfValue>> GetDutyShiftPoliciesAsync()
        {
            return await GetListOfValuesAsync("DutyShift");
        }

        public ICollection<ListOfValue> GetListOfValues(string type)
        {
            return _context.ListOfValues
                .Where(l => l.Type == type)
                .Where(l => l.Active == "Yes")
                .ToList();
        }

        public async Task<ICollection<ListOfValue>> GetListOfValuesAsync(string type)
        {
            return await _context.ListOfValues
                .Where(l => l.Type == type)
                .Where(l => l.Active == "Yes")
                .ToListAsync();
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
                        stringList.Add(listOfValue.LIC);
                        break;

                    default:
                        stringList.Add(listOfValue.DisplayValue);
                        break;
                }
            }

            return stringList;
        }

        public async Task DeleteAsync(ListOfValue value)
        {
            _context.ListOfValues.Remove(value);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ListOfValue value)
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(ListOfValue value)
        {
            _context.ListOfValues.Add(value);
            await _context.SaveChangesAsync();
        }
    }
}