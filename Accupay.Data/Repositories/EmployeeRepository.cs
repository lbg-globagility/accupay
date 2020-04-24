using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeRepository
    {
        public class EmployeeBuilder : IDisposable
        {
            private PayrollContext _context;
            private IQueryable<Employee> _query;

            public EmployeeBuilder(int organizationId, ILoggerFactory loggerFactory = null)
            {
                if (loggerFactory != null)
                {
                    _context = new PayrollContext(loggerFactory);
                }
                else
                {
                    _context = new PayrollContext();
                }
                _query = _context.Employees.Where(e => e.OrganizationID == organizationId);
            }

            public EmployeeBuilder(ILoggerFactory loggerFactory = null)
            {
                if (loggerFactory != null)
                {
                    _context = new PayrollContext(loggerFactory);
                }
                else
                {
                    _context = new PayrollContext();
                }
                _query = _context.Employees;
            }

            #region Builder Methods

            public EmployeeBuilder IsActive()
            {
                _query = _query.Where(x => x.IsActive);
                return this;
            }

            public EmployeeBuilder ByEmployeeNumber(string employeeNumber)
            {
                _query = _query.Where(x => x.EmployeeNo == employeeNumber);
                return this;
            }

            public EmployeeBuilder ByEmployeeNumbers(string[] employeeNumbers)
            {
                _query = _query.Where(x => employeeNumbers.Contains(x.EmployeeNo));
                return this;
            }

            public EmployeeBuilder ByIds(List<int?> rowIds)
            {
                _query = _query.Where(x => rowIds.Contains(x.RowID));
                return this;
            }

            public EmployeeBuilder HasPaystubs(int payPeriodId)
            {
                _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: true));
                return this;
            }

            public EmployeeBuilder HasNoPaystubs(int payPeriodId)
            {
                _query = _query.Where(CheckIfEmployeeHasPaystub(payPeriodId: payPeriodId, expected: false));
                return this;
            }

            public EmployeeBuilder IncludePayFrequency()
            {
                _query = _query.Include(x => x.PayFrequency);
                return this;
            }

            public EmployeeBuilder IncludePosition()
            {
                _query = _query.Include(x => x.Position);
                // Note: No need to call WithPosition if WithDivision is already called.
                return this;
            }

            public EmployeeBuilder IncludeDivision()
            {
                _query = _query.Include(x => x.Position.Division);
                return this;
            }

            public EmployeeBuilder IncludeBranch()
            {
                _query = _query.Include(x => x.Branch);
                return this;
            }

            #endregion Builder Methods

            public IEnumerable<Employee> ToList()
            {
                return _query.ToList();
            }

            public async Task<IEnumerable<Employee>> ToListAsync()
            {
                return await _query.ToListAsync();
            }

            public Employee FirstOrDefault(int? employeeId = null)
            {
                if (employeeId != null)
                {
                    _query = _query.Where(x => x.RowID == employeeId);
                }

                return _query.FirstOrDefault();
            }

            public async Task<Employee> FirstOrDefaultAsync(int? employeeId = null)
            {
                if (employeeId != null)
                {
                    _query = _query.Where(x => x.RowID == employeeId);
                }

                return await _query.FirstOrDefaultAsync();
            }

            public void Dispose()
            {
                _context.Dispose();
            }

            private Expression<Func<Employee, bool>> CheckIfEmployeeHasPaystub(int payPeriodId, bool expected)
            {
                return e => _context.Paystubs.Any(p => p.EmployeeID.Value == e.RowID.Value &&
                                                       p.PayPeriodID.Value == payPeriodId) == expected;
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllActiveAsync(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.IsActive().
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetByEmployeeNumbersAsync(string[] employeeNumbers)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.
                    ByEmployeeNumbers(employeeNumbers).
                    ToListAsync();
            }
        }

        public async Task<Employee> GetByIdAsync(int? rowID)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.FirstOrDefaultAsync(rowID);
            }
        }

        public async Task<Employee> GetByIdWithPayFrequencyAsync(int? rowID)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.
                    IncludePayFrequency().
                    FirstOrDefaultAsync(rowID);
            }
        }

        public async Task<IEnumerable<Employee>> GetByManyIdAsync(List<int?> rowIDs)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.
                    ByIds(rowIDs).
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllWithPayrollAsync(int payPeriodId, int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.HasPaystubs(payPeriodId).
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithoutPayrollAsync(int payPeriodId, int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.HasNoPaystubs(payPeriodId).
                                        IsActive().
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllWithPositionAsync(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId, PayrollContext.DbCommandConsoleLoggerFactory))
            {
                return await builder.IncludePosition().ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithPositionAsync(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.IncludePosition().
                                    IsActive().
                                    ToListAsync();
            }
        }

        #region By Employee

        public async Task<Employee> GetActiveEmployeeWithDivisionAsync(int? employeeId)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.IncludeDivision().
                                    IsActive().
                                    FirstOrDefaultAsync(employeeId);
            }
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.IncludePosition().
                                        ByEmployeeNumber(employeeNumber).
                                        FirstOrDefaultAsync();
            }
        }

        #endregion By Employee

        public async Task<IEnumerable<Employee>> SearchSimpleLocal(IEnumerable<Employee> employees, string searchValue)
        {
            if (employees == null || employees.Count() == 0)
                return employees;

            Func<Employee, bool> matchCriteria = (Employee employee) =>
            {
                var containsEmployeeId = employee.EmployeeNo.ToLower().Contains(searchValue);
                var containsFullName = (employee.FirstName.ToLower() + " " + employee.LastName.ToLower()).Contains(searchValue);

                var reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower();
                var containsFullNameInReverse = reverseFullName.Contains(searchValue);

                return containsEmployeeId | containsFullName | containsFullNameInReverse;
            };

            return await Task.Run(() => employees.Where(matchCriteria).ToList());
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