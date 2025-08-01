using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class ListOfValueRepository : SavableRepository<ListOfValue>, IListOfValueRepository
    {
        public ListOfValueRepository(PayrollContext context) : base(context)
        {
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

        public async Task<ICollection<ListOfValue>> GetDutyReportProvidersAsync()
        {
            return await GetListOfValuesAsync("ReportProviders", checkIfActive: false);
        }

        public ICollection<ListOfValue> GetListOfValues(string type, bool checkIfActive = true)
        {
            IQueryable<ListOfValue> query = CreateBaseGetListOfValues(type, checkIfActive);

            return query.ToList();
        }

        public async Task<ICollection<ListOfValue>> GetListOfValuesAsync(string type, bool checkIfActive = true)
        {
            IQueryable<ListOfValue> query = CreateBaseGetListOfValues(type, checkIfActive);

            return await query.ToListAsync();
        }

        public async Task<ListOfValue> GetPolicyAsync(string type, string lic, int organizationId)
        {
            return await _context.ListOfValues
                .AsNoTracking()
                .Where(f => f.Type == type)
                .Where(f => f.LIC == lic)
                .Where(f => f.OrganizationID == organizationId)
                .FirstOrDefaultAsync();
        }

        private IQueryable<ListOfValue> CreateBaseGetListOfValues(string type, bool checkIfActive)
        {
            var query = _context.ListOfValues
                .Where(l => l.Type == type);

            if (checkIfActive)
            {
                query = query.Where(l => l.Active == ListOfValue.ActiveYesOption);
            }

            return query;
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
    }
}
