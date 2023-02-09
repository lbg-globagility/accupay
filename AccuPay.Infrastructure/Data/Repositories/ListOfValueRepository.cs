using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Policies;
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

        public async Task<ListOfValue> GetListOfValueOfWeeklyPayPeriodPolicyByOrganization(int organizationId) =>
            await _context.ListOfValues
            .AsNoTracking()
            .Where(f => f.Type == PolicyHelper.WeeklyPayPeriodPolicyType)
            .Where(f => f.OrganizationID == organizationId)
            .FirstOrDefaultAsync();

        public async Task<WeeklyPayPeriodPolicy> GetWeeklyPayPeriodPolicyByOrganization(int organizationId, int year)
        {
            var listOfValue = await GetListOfValueOfWeeklyPayPeriodPolicyByOrganization(organizationId: organizationId);
            return new WeeklyPayPeriodPolicy(listOfValue: listOfValue, year: year);
        }

        public async Task CreateIfNotExistsAsync(int userId,
            string type,
            string lic,
            string displayValue,
            bool isByOrganization = false,
            int? organizationId = null)
        {
            ListOfValue listOfValue;

            if (isByOrganization)
            {
                listOfValue = await _context.ListOfValues
                    .AsNoTracking()
                    .Where(l => l.OrganizationID == organizationId)
                    .Where(l => l.Type == type)
                    .Where(l => l.LIC == lic)
                    .FirstOrDefaultAsync();
            }
            else
            {
                listOfValue = await _context.ListOfValues
                    .AsNoTracking()
                    .Where(l => l.Type == type)
                    .Where(l => l.LIC == lic)
                    .FirstOrDefaultAsync();
            }

            if (listOfValue == null)
            {
                listOfValue = new ListOfValue()
                {
                    LIC = lic,
                    DisplayValue = displayValue,
                    Type = type,
                    CreatedBy = userId
                };

                if (isByOrganization) listOfValue.OrganizationID = organizationId;

                await CreateAsync(listOfValue);
            }
        }
    }
}
