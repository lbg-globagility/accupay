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

        private readonly EmployeeQueryBuilder builder;

        public EmployeeRepository(PayrollContext context, EmployeeQueryBuilder builder)
        {
            _context = context;

            this.builder = builder;
        }

        #region CRUD

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
            return await builder.
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveAsync(int organizationId)
        {
            return await builder.
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllWithPayrollAsync(int payPeriodId,
                                                                    int organizationId)
        {
            return await builder.
                            HasPaystubs(payPeriodId).
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithoutPayrollAsync(int payPeriodId, int organizationId)
        {
            return await builder.
                            HasNoPaystubs(payPeriodId).
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllWithPositionAsync(int organizationId)
        {
            return await builder.
                            IncludePosition().
                            ToListAsync(organizationId);
        }

        public IEnumerable<Employee> GetAllActiveWithPosition(int organizationId)
        {
            return builder.
                        IncludePosition().
                        IsActive().
                        ToList(organizationId);
        }

        public IEnumerable<Employee> GetAllWithDivisionAndPosition(int organizationId)
        {
            return builder.
                        IncludeDivision().
                        ToList(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetAllActiveWithDivisionAndPositionAsync(int organizationId)
        {
            return await builder.
                            IncludeDivision().
                            IsActive().
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetByMultipleEmployeeNumberAsync(string[] employeeNumbers,
                                                                                int organizationId)
        {
            return await builder.
                            Filter(x => employeeNumbers.Contains(x.EmployeeNo)).
                            ToListAsync(organizationId);
        }

        public async Task<IEnumerable<Employee>> GetByMultipleIdAsync(int[] employeeIdList)
        {
            return await builder.
                            Filter(x => employeeIdList.Contains(x.RowID.Value)).
                            ToListAsync(null);
        }

        public async Task<IEnumerable<Employee>> GetByPositionAsync(int positionId)
        {
            return await builder.
                            Filter(x => x.PositionID == positionId).
                            ToListAsync(null);
        }

        public async Task<IEnumerable<Employee>> GetByBranchAsync(int branchId)
        {
            return await builder.
                            Filter(x => x.BranchID == branchId).
                            ToListAsync(null);
        }

        #endregion List of entities

        #region Single entity

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await builder.
                            GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetActiveEmployeeWithDivisionAndPositionAsync(int employeeId)
        {
            return await builder.
                            IncludeDivision().
                            IsActive().
                            GetByIdAsync(employeeId, null);
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber, int organizationId)
        {
            return await builder.
                            ByEmployeeNumber(employeeNumber).
                            FirstOrDefaultAsync(organizationId);
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