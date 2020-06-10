using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeRepository
    {
        private readonly PayrollContext _context;

        public EmployeeRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task SaveAsync(Employee employee)
        {
            if (employee.RowID.HasValue)
            {
                _context.Entry(employee).State = EntityState.Modified;
            }
            else
            {
                _context.Employees.AddRange(employee);
            }
            await _context.SaveChangesAsync();
        }

        public async Task Attach(Employee employee)
        {
            _context.Employees.Attach(employee);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedListResult<Employee>> GetPaginatedListAsync(PageOptions options, int organizationId, string searchTerm = "")
        {
            var query = _context.Employees
                                .Include(e => e.Position)
                                .Where(e => e.OrganizationID == organizationId)
                                .OrderBy(e => e.FullNameLastNameFirst)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.FullNameWithMiddleInitialLastNameFirst, searchTerm) ||
                    EF.Functions.Like(x.EmployeeNo, searchTerm));
            }

            var employees = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Employee>(employees, count);
        }

        public async Task SaveManyAsync(List<Employee> employees)
        {
            var updated = employees.Where(e => e.RowID.HasValue).ToList();
            if (updated.Any())
            {
                updated.ForEach(x => _context.Entry(x).State = EntityState.Modified);
            }

            var added = employees.Where(e => !e.RowID.HasValue).ToList();
            if (added.Any())
            {
                // this adds a value to RowID (int minimum value)
                // so if there is a code checking for null to RowID
                // it will always be false
                _context.Employees.AddRange(added);
            }
            await _context.SaveChangesAsync();
        }

        #endregion CRUD

        #region Queries

        #region List of entities

        public async Task<IEnumerable<Employee>> GetAllAsync(int organizationId)
        {
            // builder's lifecycle should only span in each method
            // that is why we did not inject this on the constructor
            // and create a class variable of EmployeeQueryBuilder
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<PaginatedListResult<Employee>> GetPaginatedListWithTimeEntryAsync(
            PageOptions options,
            int organizationId,
            int payPeriodId,
            string searchTerm = "")
        {
            return await BaseGetPaginatedListAsync(
                options,
                organizationId: organizationId,
                payPeriodId: payPeriodId,
                searchTerm: searchTerm);
        }

        private async Task<PaginatedListResult<Employee>> BaseGetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            int? payPeriodId = null,
            bool hasPaystubs = false,
            string searchTerm = "")
        {
            var query = _context.Employees
                .Where(x => x.OrganizationID == organizationId)
                .OrderByDescending(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ThenBy(x => x.EmployeeNo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LastName, searchTerm) ||
                    EF.Functions.Like(x.FirstName, searchTerm) ||
                    EF.Functions.Like(x.EmployeeNo, searchTerm));
            }

            if (payPeriodId.HasValue)
            {
                if (hasPaystubs)
                {
                    query = query.Where(e => e.Paystubs.Where(p => p.PayPeriodID == payPeriodId).Any());
                }
            }

            var employees = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<Employee>(employees, count);
        }

        public async Task<IEnumerable<Employee>> GetAllWithPayrollAsync(int payPeriodId,
                                                                    int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            HasPaystubs(payPeriodId).
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithoutPayrollAsync(int payPeriodId, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            HasNoPaystubs(payPeriodId).
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllWithPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            IncludePosition().
                            ToListAsync(organizationId);
        }

        public IEnumerable<Employee> GetAllActiveWithPosition(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return builder.
                        IncludePosition().
                        IsActive().
                        ToList(organizationId);
        }

        public IEnumerable<Employee> GetAllWithDivisionAndPosition(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return builder.
                        IncludeDivision().
                        ToList(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithDivisionAndPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            IncludeDivision().
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetByMultipleEmployeeNumberAsync(string[] employeeNumbers,
                                                                                int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            Filter(x => employeeNumbers.Contains(x.EmployeeNo)).
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetByMultipleIdAsync(int[] employeeIdList)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            Filter(x => employeeIdList.Contains(x.RowID.Value)).
                            ToListAsync(null);
        }

        public async Task<IEnumerable<Employee>> GetByPositionAsync(int positionId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            Filter(x => x.PositionID == positionId).
                            ToListAsync(null);
        }

        public async Task<IEnumerable<Employee>> GetByBranchAsync(int branchId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            Filter(x => x.BranchID == branchId).
                            ToListAsync(null);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithoutImageAsync(int organizationId)
        {
            return await _context.Employees
                                 .Include(x => x.OriginalImage)
                                 .Where(x => x.OrganizationID == organizationId)
                                 .Where(x => x.OriginalImageId == null)
                                 .ToListAsync();
        }

        #endregion List of entities

        #region Single entity

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetActiveEmployeeWithDivisionAndPositionAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            IncludeDivision().
                            IsActive().
                            GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.
                            ByEmployeeNumber(employeeNumber).
                            FirstOrDefaultAsync(organizationId);
        }

        public async Task<string> GetImagePathByIdAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            var employee = await builder.
                IncludeImage().
                GetByIdAsync(employeeId, null);

            return employee.OriginalImage?.Path;
        }

        #endregion Single entity

        #region Others

        public async Task<Salary> GetCurrentSalaryAsync(int employeeId, DateTime? date = null)
        {
            date = date ?? DateTime.Now;

            return await _context.Salaries.
                            Where(x => x.EmployeeID == employeeId).
                            Where(x => x.EffectiveFrom <= date).
                            OrderByDescending(x => x.EffectiveFrom).
                            FirstOrDefaultAsync();
        }

        public async Task<decimal> GetVacationLeaveBalance(int employeeId)
        {
            return await GetLeaveBalance(employeeId, ProductConstant.VACATION_LEAVE);
        }

        public async Task<decimal> GetSickLeaveBalance(int employeeId)
        {
            return await GetLeaveBalance(employeeId, ProductConstant.SICK_LEAVE);
        }

        private async Task<decimal> GetLeaveBalance(int employeeId, string partNo)
        {
            var defaultBalance = 0;

            var leaveledger = await _context.LeaveLedgers.
                                            Include(l => l.Product).
                                            Where(l => l.Product.PartNo == partNo).
                                            Where(l => l.EmployeeID == employeeId).
                                            FirstOrDefaultAsync();

            if (leaveledger?.LastTransactionID == null)
                return defaultBalance;

            var leaveTransaction = await _context.LeaveTransactions.
                                            Where(l => l.RowID == leaveledger.LastTransactionID).
                                            FirstOrDefaultAsync();

            if (leaveTransaction?.Balance == null)
                return defaultBalance;

            return leaveTransaction.Balance;
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