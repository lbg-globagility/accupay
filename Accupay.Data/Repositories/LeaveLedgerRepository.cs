using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class LeaveLedgerRepository
    {
        private readonly PayrollContext context;

        public LeaveLedgerRepository(PayrollContext context)
        {
            this.context = context;
        }

        public async Task<ICollection<LeaveLedger>> GetAllByEmployee(int? employeeId)
        {
            return await context.LeaveLedgers
                .Where(t => t.EmployeeID == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<LeaveTransaction>> GetTransactionsByLedger(int? leaveLedgerId)
        {
            return await context.LeaveTransactions
                .Where(t => t.LeaveLedgerID == leaveLedgerId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<PaginatedListResult<LeaveTransaction>> GetPaginatedListLedger(PageOptions options, int organizationId, int id, string type = null)
        {
            var query = context.LeaveTransactions
                                .Include(x => x.Employee)
                                .Include(x => x.LeaveLedger)
                                .Where(x => x.OrganizationID == organizationId)
                                .Where(x => x.EmployeeID == id)
                                .Where(x => x.LeaveLedger.Product.PartNo == type)
                                .OrderByDescending(x => x.TransactionDate)
                                .AsQueryable();

            var transaction = await query.Page(options).ToListAsync();
            var count = await query.CountAsync();

            return new PaginatedListResult<LeaveTransaction>(transaction, count);
        }

        public async Task<IEnumerable<LeaveLedger>> GetLeaveBalance(int organizationId, string searchTerm = null)
        {
            //var distinct = context.LeaveLedgers
            //                    .Include(x => x.Product)
            //                    .Where(x => x.OrganizationID == organizationId)
            //                    .Where(x => x.Product.PartNo == ProductConstant.VACATION_LEAVE || x.Product.PartNo == ProductConstant.SICK_LEAVE)
            //                    .Where(x => x.LastTransactionID != null)
            //                    .Select(x => x.EmployeeID)
            //                    .Distinct();

            //var items = await distinct.Page(options).ToListAsync();

            var query = await context.LeaveLedgers
                                .Include(x => x.LastTransaction.Employee)
                                .Include(x => x.Product)
                                .Where(x => x.OrganizationID == organizationId)
                                .Where(x => x.Product.PartNo == ProductConstant.VACATION_LEAVE || x.Product.PartNo == ProductConstant.SICK_LEAVE)
                                .Where(x => x.LastTransactionID != null)
                                .OrderBy(x => x.LastTransaction.Employee.LastName)
                                .ThenBy(x => x.LastTransaction.Employee.FirstName)
                                .ToListAsync();

            var test = query.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm}%";

                test = test.Where(x =>
                    EF.Functions.Like(x.LastTransaction.Employee.FullNameWithMiddleInitialLastNameFirst, searchTerm) ||
                    EF.Functions.Like(x.EmployeeID.ToString(), searchTerm));
            }

            return test;

            //var final = await query.Page(options).ToListAsync();
            //var count = await distinct.CountAsync();

            //var test = await query.ToListAsync();

            //return new PaginatedListResult<LeaveLedger>(test, count);
        }
    }
}