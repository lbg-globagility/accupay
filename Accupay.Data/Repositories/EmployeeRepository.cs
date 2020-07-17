using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class EmployeeRepository : BaseRepository
    {
        private readonly PayrollContext _context;

        public EmployeeRepository(PayrollContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task SaveAsync(Employee employee)
        {
            if (IsNewEntity(employee.RowID))
            {
                _context.Employees.AddRange(employee);
            }
            else
            {
                _context.Entry(employee).State = EntityState.Modified;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<Employee>> GetPaginatedListAsync(EmployeePageOptions options, int organizationId)
        {
            var query = _context.Employees
                .Include(e => e.Position)
                .Where(e => e.OrganizationID == organizationId)
                .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ThenBy(e => e.MiddleName)
                .AsQueryable();

            if (options.HasSearchTerm)
            {
                var searchTerm = $"%{options.SearchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LastName + " " + x.FirstName, searchTerm) ||
                    EF.Functions.Like(x.EmployeeNo, searchTerm));
            }

            if (options.HasFilter)
            {
                if (options.Filter == "Active only")
                {
                    query = query.Where(e => e.EmploymentStatus != "Resigned" && e.EmploymentStatus != "Terminated");
                }
            }

            if (options.HasPosition)
            {
                query = query.Where(e => e.PositionID == options.PositionId);
            }

            var employees = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Employee>(employees, count);
        }

        public async Task SaveManyAsync(List<Employee> employees)
        {
            var updated = employees.Where(e => !IsNewEntity(e.RowID)).ToList();
            if (updated.Any())
            {
                updated.ForEach(x => _context.Entry(x).State = EntityState.Modified);
            }

            var added = employees.Where(e => IsNewEntity(e.RowID)).ToList();
            if (added.Any())
            {
                _context.Employees.AddRange(added);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ChangeManyAsync(List<Employee> added = null,
                                          List<Employee> updated = null,
                                          List<Employee> deleted = null)
        {
            if (added != null)
            {
                added.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Added;
                });
            }

            if (updated != null)
            {
                updated.ForEach(shift =>
                {
                    _context.Entry(shift).State = EntityState.Modified;
                });
            }

            if (deleted != null)
            {
                deleted = deleted
                    .GroupBy(x => x.RowID)
                    .Select(x => x.FirstOrDefault())
                    .ToList();
                _context.Employees.RemoveRange(deleted);
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

        public async Task<PaginatedList<Employee>> GetPaginatedListWithTimeEntryAsync(
            PageOptions options,
            int organizationId,
            int payPeriodId,
            string searchTerm = "")
        {
            var payPeriod = await _context.PayPeriods.FirstOrDefaultAsync(x => x.RowID == payPeriodId);

            var paginatedListResult = await BaseGetPaginatedListAsync(
                                                options,
                                                organizationId: organizationId,
                                                payPeriod: payPeriod,
                                                hasTimeEntries: true,
                                                searchTerm: searchTerm);

            var employeeIds = paginatedListResult.Items.Select(x => x.RowID.Value);
            var timeEntries = _context.TimeEntries
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => payPeriod.PayFromDate <= x.Date)
                .Where(x => x.Date <= payPeriod.PayToDate)
                .Where(x => employeeIds.Contains(x.EmployeeID.Value))
                .ToList();

            foreach (var employee in paginatedListResult.Items)
            {
                employee.TimeEntries = timeEntries.Where(x => x.EmployeeID == employee.RowID).ToList();
            }

            return paginatedListResult;
        }

        private async Task<PaginatedList<Employee>> BaseGetPaginatedListAsync(
            PageOptions options,
            int organizationId,
            PayPeriod payPeriod = null,
            bool hasPaystubs = false,
            bool hasTimeEntries = false,
            string searchTerm = "")
        {
            var query = _context.Employees
                .Where(x => x.OrganizationID == organizationId)
                .OrderBy(x => x.LastName)
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

            if (payPeriod != null)
            {
                if (hasPaystubs)
                {
                    query = query.Where(e => e.Paystubs.Where(p => p.PayPeriodID == payPeriod.RowID).Any());
                }

                if (hasTimeEntries)
                {
                    query = query.Where(e => e.TimeEntries
                                                .Where(t => t.Date >= payPeriod.PayFromDate)
                                                .Where(t => t.Date <= payPeriod.PayToDate)
                                                .Any());
                }
            }

            var employees = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Employee>(employees, count);
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

        public async Task<IEnumerable<Employee>> GetEmployeesWithoutImageAsync()
        {
            return await _context.Employees
                .Include(x => x.OriginalImage)
                .Where(x => x.OriginalImageId == null)
                .ToListAsync();
        }

        public async Task<ICollection<string>> GetEmploymentStatuses()
        {
            var listOfValues = await _context.ListOfValues
                .Where(t => t.Type == "Employment Status")
                .ToListAsync();

            return listOfValues.Select(t => t.DisplayValue).ToList();
        }

        #endregion List of entities

        #region Single entity

        public Employee GetById(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return builder.IncludePosition().IncludeEmploymentPolicy().GetById(employeeId, null);
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.IncludePosition().IncludeEmploymentPolicy().GetByIdAsync(employeeId, null);
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
            return await CreateBaseQueryCurrentSalary(employeeId, date)
                            .FirstOrDefaultAsync();
        }

        public Salary GetCurrentSalary(int employeeId, DateTime? date = null)
        {
            return CreateBaseQueryCurrentSalary(employeeId, date)
                            .FirstOrDefault();
        }

        private IOrderedQueryable<Salary> CreateBaseQueryCurrentSalary(int employeeId, DateTime? date = null)
        {
            date = date ?? DateTime.Now;

            return _context.Salaries
                            .Where(x => x.EmployeeID == employeeId)
                            .Where(x => x.EffectiveFrom <= date)
                            .OrderByDescending(x => x.EffectiveFrom);
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