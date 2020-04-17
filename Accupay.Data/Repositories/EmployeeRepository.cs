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
        public class EmployeeBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<Employee> _query;

            public EmployeeBuilder()
            {
                _context = new PayrollContext();
                _query = _context.Employees;
            }

            #region Builder Methods

            public EmployeeBuilder WithPayFrequency()
            {
                _query = _query.Include(x => x.PayFrequency);
                return this;
            }

            public EmployeeBuilder WithPosition()
            {
                _query = _query.Include(x => x.Position);
                // Note: No need to call WithPosition if WithDivision is already called.
                return this;
            }

            public EmployeeBuilder WithDivision()
            {
                _query = _query.Include(x => x.Position.Division);
                return this;
            }

            public EmployeeBuilder WithBranch()
            {
                _query = _query.Include(x => x.Branch);
                return this;
            }

            #endregion Builder Methods

            public IEnumerable<Employee> ToList()
            {
                return _query.ToList();
            }

            public Employee FirstOrDefault(int? employeeId = null)
            {
                if (employeeId != null)
                {
                    _query = _query.Where(x => x.RowID == employeeId);
                }

                return _query.FirstOrDefault();
            }

            public void Dispose()
            {
                _context.Dispose();
            }
        }
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
    }
}