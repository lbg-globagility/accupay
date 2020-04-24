using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeRepository
    {
        public async Task<List<Employee>> GetAllAsync(int organizationID)
        {
            using (var context = new PayrollContext())
            {
                return await GetAllEmployeeBaseQuery(organizationID, context).ToListAsync();
            }
        }

        public async Task<List<Employee>> GetAllActiveAsync(int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Where(l => l.IsActive).ToListAsync();
            }
        }

        public async Task<List<Employee>> GetAllActiveWithoutPayrollAsync(int? payPeriodId, int organizationID)
        {
            //PayrollContext.DbCommandConsoleLoggerFactory
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context).Where(l => l.IsActive);

                return await query.Where(e => context.Paystubs.Any(p => p.EmployeeID.Value == e.RowID.Value && p.PayPeriodID.Value == payPeriodId.Value) == false).ToListAsync();
            }
        }

        public async Task<List<Employee>> GetAllWithPayrollAsync(int payPeriodId, int organizationID)
        {
            //PayrollContext.DbCommandConsoleLoggerFactory
            using (var context = new PayrollContext())
            {
                return await GetAllEmployeeBaseQuery(organizationID, context).Where(e => context.Paystubs.Any(p => p.EmployeeID.Value == e.RowID.Value && p.PayPeriodID.Value == payPeriodId)).ToListAsync();
            }
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Where(l => l.EmployeeNo == employeeNumber).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Employee>> GetByEmployeeNumbersAsync(string[] employeeNumbers, int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Where(l => employeeNumbers.Contains(l.EmployeeNo)).ToListAsync();
            }
        }

        public async Task<Employee> GetByIdAsync(int organizationID, int rowID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Where(e => e.RowID == rowID).FirstOrDefaultAsync();
            }
        }

        public async Task<Employee> GetByIdWithPayFrequencyAsync(int organizationID, int rowID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Include(e => e.PayFrequency).Where(e => e.RowID == rowID).FirstOrDefaultAsync();
            }
        }

        public async Task<List<Employee>> GetByManyIdAsync(int organizationID, List<int> rowIDs)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Where(e => rowIDs.Contains(e.RowID.Value)).ToListAsync();
            }
        }

        public async Task<List<Employee>> GetAllWithPositionAsync(int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Include(e => e.Position).ToListAsync();
            }
        }

        public async Task<List<Employee>> GetAllActiveWithPositionAsync(int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Include(e => e.Position).Where(l => l.IsActive).ToListAsync();
            }
        }

        public async Task<Employee> GetEmployeeWithDivisionAsync(int? id, int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var query = GetAllEmployeeBaseQuery(organizationID, context);

                return await query.Include(e => e.Position.Division).Where(l => l.RowID.Value == id.Value).Where(l => l.IsActive).FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<Employee>> SearchSimpleLocal(IEnumerable<Employee> employees, string searchValue)
        {
            if (employees == null || employees.Count() == 0)
                return employees;

            //var f = (ds) => { };

            //bool matchCriteria = delegate (Employee employee) =>
            //{
            //    var containsEmployeeId = employee.EmployeeNo.ToLower().Contains(searchValue);
            //    var containsFullName = (employee.FirstName.ToLower() + " " + employee.LastName.ToLower()).Contains(searchValue);

            //    var reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower();
            //    var containsFullNameInReverse = reverseFullName.Contains(searchValue);

            //    return containsEmployeeId | containsFullName | containsFullNameInReverse;
            //};

            return await Task.Run(() => employees.ToList());
        }

        private IQueryable<Employee> GetAllEmployeeBaseQuery(int organizationID, PayrollContext context)
        {
            return context.Employees.Where(e => e.OrganizationID == organizationID);
        }

        public async Task SaveManyAsync(int organizationID, int userID, List<Employee> employees)
        {
            using (PayrollContext context = new PayrollContext())
            {
                var added = employees.Where(e => !e.RowID.HasValue).ToList();
                if (added.Any())
                {
                    context.Employees.AddRange(added);
                }

                var updated = employees.Where(e => e.RowID.HasValue).ToList();
                if (updated.Any())
                {
                    await UpdateManyAsync(employees, context);
                }
                await context.SaveChangesAsync();
            }
        }

        private async Task UpdateManyAsync(List<Employee> employees, PayrollContext context)
        {
            var employeeIds = employees.Select(e => e.RowID).ToArray();

            var employeesToUpdate = await context.Employees.
                Where(e => employeeIds.Contains(e.RowID)).
                ToListAsync();
            if (!employeesToUpdate.Any()) return;

            foreach (var e in employeesToUpdate)
            {
                var updatedEmployee = employees.FirstOrDefault(ee => ee.RowID == e.RowID);

                ApplyChanges(e, updatedEmployee);
            }
        }

        private void ApplyChanges(Employee toBeUpdateEmployee, Employee updatedEmployee)
        {
            throw new NotImplementedException();
        }
    }
}