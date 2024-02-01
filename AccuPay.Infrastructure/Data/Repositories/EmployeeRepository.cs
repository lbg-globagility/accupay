using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class EmployeeRepository : SavableRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollContext context) : base(context)
        {
        }

        #region Queries

        #region List of entities

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

        public async Task<ICollection<Employee>> GetAllAsync(int organizationId)
        {
            // builder's lifecycle should only span in each method
            // that is why we did not inject this on the constructor
            // and create a class variable of EmployeeQueryBuilder
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllActiveAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IsActive()
                .ToListAsync(organizationId);
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

        public async Task<ICollection<Employee>> GetAllWithPayrollAsync(int payPeriodId, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .HasPaystubs(payPeriodId)
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllActiveWithoutPayrollAsync(int payPeriodId, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .HasNoPaystubs(payPeriodId)
                .IsActive()
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllWithPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllActiveWithPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .IsActive()
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllWithinServicePeriodAsync(int organizationId, DateTime currentDate)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .WithinServicePeriod(currentDate)
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllWithinServicePeriodWithPositionAsync(int organizationId, DateTime currentDate)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludePosition()
                .WithinServicePeriod(currentDate)
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllWithDivisionAndPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludeDivision()
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetAllActiveWithDivisionAndPositionAsync(int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludeDivision()
                .IsActive()
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetByMultipleEmployeeNumberAsync(string[] employeeNumbers, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .Filter(x => employeeNumbers.Contains(x.EmployeeNo))
                .ToListAsync(organizationId);
        }

        public async Task<ICollection<Employee>> GetByMultipleIdAsync(int[] employeeIdList)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .Filter(x => employeeIdList.Contains(x.RowID.Value))
                .ToListAsync(null);
        }

        public async Task<ICollection<Employee>> GetByPositionAsync(int positionId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .Filter(x => x.PositionID == positionId)
                .ToListAsync(null);
        }

        public async Task<ICollection<Employee>> GetByBranchAsync(int branchId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .Filter(x => x.BranchID == branchId)
                .ToListAsync(null);
        }

        public async Task<ICollection<Employee>> GetEmployeesWithoutImageAsync()
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

        public async Task<PaginatedList<Employee>> GetUnregisteredEmployeeAsync(PageOptions options, string searchTerm, int clientId, int organizationId)
        {
            var registeredEmployeeIds = await _context.Users
                .Where(u => u.EmployeeId != null)
                .Where(u => u.ClientId == clientId)
                .Select(u => u.EmployeeId.Value)
                .ToListAsync();

            var query = _context.Employees
                .Where(x => x.OrganizationID == organizationId)
                .Where(e => !registeredEmployeeIds.Contains(e.RowID.Value))
                .OrderBy(x => x.FullNameLastNameFirst)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.LastName, searchTerm) ||
                    EF.Functions.Like(x.FirstName, searchTerm) ||
                    EF.Functions.Like(x.EmployeeNo, searchTerm));
            }

            var employees = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedList<Employee>(employees, count);
        }

        #endregion List of entities

        #region Single entity

        public override async Task<Employee> GetByIdAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder.IncludePosition().IncludeEmploymentPolicy().GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetActiveEmployeeWithDivisionAndPositionAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .IncludeDivision()
                .IsActive()
                .GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            return await builder
                .ByEmployeeNumber(employeeNumber)
                .FirstOrDefaultAsync(organizationId);
        }

        public async Task<string> GetImagePathByIdAsync(int employeeId)
        {
            var builder = new EmployeeQueryBuilder(_context);
            var employee = await builder
                .IncludeImage()
                .GetByIdAsync(employeeId, null);

            return employee.OriginalImage?.Path;
        }

        #endregion Single entity

        #region Others

        public async Task<Salary> GetCurrentSalaryAsync(int employeeId, DateTime? cutOffEnd = null)
        {
            return await CreateBaseQueryCurrentSalary(employeeId, cutOffEnd)
                .FirstOrDefaultAsync();
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

            var leaveledger = await _context.LeaveLedgers
                .Include(l => l.Product)
                .Where(l => l.Product.PartNo == partNo)
                .Where(l => l.EmployeeID == employeeId)
                .FirstOrDefaultAsync();

            if (leaveledger?.LastTransactionID == null)
                return defaultBalance;

            var leaveTransaction = await _context.LeaveTransactions
                .Where(l => l.RowID == leaveledger.LastTransactionID)
                .FirstOrDefaultAsync();

            if (leaveTransaction?.Balance == null)
                return defaultBalance;

            return leaveTransaction.Balance;
        }

        private IOrderedQueryable<Salary> CreateBaseQueryCurrentSalary(int employeeId, DateTime? cutOffEnd)
        {
            var query = _context.Salaries.Where(x => x.EmployeeID == employeeId);
            return SalaryQueryHelper.GetLatestSalaryQuery(query, cutOffEnd);
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
                var containsEmployeeId = employee.EmployeeNo != null && employee.EmployeeNo.ToLower().Contains(searchValue);
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
