using AccuPay.Data.Entities;
using AccuPay.Utilities.Extensions;
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
        #region CRUD

        public async Task SaveManyAsync(List<Employee> employees)
        {
            using (PayrollContext context = new PayrollContext())
            {
                var updated = employees.Where(e => e.RowID.HasValue).ToList();
                if (updated.Any())
                {
                    updated.ForEach(x => context.Entry(x).State = EntityState.Modified);
                }

                var added = employees.Where(e => !e.RowID.HasValue).ToList();
                if (added.Any())
                {
                    // this adds a value to RowID (int minimum value)
                    // so if there is a code checking for null to RowID
                    // it will always be false
                    context.Employees.AddRange(added);
                }
                await context.SaveChangesAsync();
            }
        }

        #endregion CRUD

        #region Queries

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
                _query = _query.Where(x => x.EmployeeNo.Trim().ToLower() ==
                                            employeeNumber.ToTrimmedLowerCase());
                return this;
            }

            public EmployeeBuilder Filter(Expression<Func<Employee, bool>> filter)
            {
                _query = _query.Where(filter);
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
                // Note: No need to call WithPosition if WithDivision is already called.
                _query = _query.Include(x => x.Position);
                return this;
            }

            public EmployeeBuilder IncludeDivision()
            {
                // Note: If needed division, position will also be queried automatically
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

            public Employee GetById(int employeeId)
            {
                return _query.Where(x => x.RowID == employeeId).FirstOrDefault();
            }

            public async Task<Employee> GetByIdAsync(int employeeId)
            {
                return await _query.Where(x => x.RowID == employeeId).FirstOrDefaultAsync();
            }

            public Employee FirstOrDefault()
            {
                return _query.FirstOrDefault();
            }

            public async Task<Employee> FirstOrDefaultAsync()
            {
                return await _query.FirstOrDefaultAsync();
            }

            public void Dispose()
            {
                // TODO: use a standard code from the internet for disposing IDisposable objects
                // and to other repository with builder pattern like User
                _context.Dispose();
            }

            private Expression<Func<Employee, bool>> CheckIfEmployeeHasPaystub(int payPeriodId, bool expected)
            {
                return e => _context.Paystubs.
                                        Any(p => p.EmployeeID == e.RowID &&
                                            p.PayPeriodID == payPeriodId) == expected;
            }
        }

        #region List of entities

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
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.IncludePosition().ToListAsync();
            }
        }

        public IEnumerable<Employee> GetAllActiveWithPosition(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return builder.IncludePosition().
                                IsActive().
                                ToList();
            }
        }

        public IEnumerable<Employee> GetAllWithDivisionAndPosition(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return builder.IncludeDivision().ToList();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithDivisionAndPositionAsync(int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.IncludeDivision().
                                        IsActive().
                                        ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetByMultipleEmployeeNumberAsync(string[] employeeNumbers,
                                                                                int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.
                    Filter(x => employeeNumbers.Contains(x.EmployeeNo)).
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetByMultipleIdAsync(int[] employeeIdList)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.
                    Filter(x => employeeIdList.Contains(x.RowID.Value)).
                    ToListAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetByPositionAsync(int positionId)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.
                                Filter(x => x.PositionID == positionId).
                                ToListAsync();
            }
        }

        #endregion List of entities

        #region Single entity

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.GetByIdAsync(employeeId);
            }
        }

        public async Task<Employee> GetActiveEmployeeWithDivisionAndPositionAsync(int employeeId)
        {
            using (var builder = new EmployeeBuilder())
            {
                return await builder.IncludeDivision().
                                    IsActive().
                                    GetByIdAsync(employeeId);
            }
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationId)
        {
            using (var builder = new EmployeeBuilder(organizationId))
            {
                return await builder.ByEmployeeNumber(employeeNumber).
                                    FirstOrDefaultAsync();
            }
        }

        #endregion Single entity

        #region Others

        public async Task<Salary> GetCurrentSalaryAsync(int employeeId, DateTime? date = null)
        {
            date = date ?? DateTime.Now;

            using (var context = new PayrollContext())
            {
                return await context.Salaries.
                                Where(x => x.EmployeeID == employeeId).
                                Where(x => x.EffectiveFrom <= date).
                                OrderByDescending(x => x.EffectiveFrom).
                                FirstOrDefaultAsync();
            }
        }

        #endregion Others

        #endregion Queries

        #region TODO: Move to service

        public async Task<IEnumerable<Employee>> SearchSimpleLocal(IEnumerable<Employee> employees, string searchValue)
        {
            if (employees == null || employees.Count() == 0)
                return employees;

            searchValue = searchValue.ToLower();

            Func<Employee, bool> matchCriteria = (Employee employee) =>
            {
                var containsEmployeeId = employee.EmployeeNo.ToLower().Contains(searchValue);
                var containsFullName = (employee.FirstName.ToLower() + " " + employee.LastName.ToLower()).Contains(searchValue);

                var reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower();
                var containsFullNameInReverse = reverseFullName.Contains(searchValue);

                return containsEmployeeId || containsFullName || containsFullNameInReverse;
            };

            return await Task.Run(() => employees.Where(matchCriteria).ToList());
        }

        #endregion TODO: Move to service
    }
}