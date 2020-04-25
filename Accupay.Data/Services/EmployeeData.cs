using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeData
    {
        public static async Task<decimal> GetVacationLeaveBalance(int? employeeId)
        {
            return await GetLeaveBalance(employeeId, ProductConstant.VACATION_LEAVE);
        }

        public static async Task<decimal> GetSickLeaveBalance(int? employeeId)
        {
            return await GetLeaveBalance(employeeId, ProductConstant.SICK_LEAVE);
        }

        private static async Task<decimal> GetLeaveBalance(int? employeeId, string partNo)
        {
            var defaultBalance = 0;

            using (var context = new PayrollContext())
            {
                var leaveledger = await context.LeaveLedgers.
                                                Include(l => l.Product).
                                                Where(l => l.Product.PartNo == partNo).
                                                Where(l => l.EmployeeID == employeeId).
                                                FirstOrDefaultAsync();

                if (leaveledger?.LastTransactionID == null)
                    return defaultBalance;

                var leaveTransaction = await context.LeaveTransactions.
                                                Where(l => l.RowID == leaveledger.LastTransactionID).
                                                FirstOrDefaultAsync();

                if (leaveTransaction?.Balance == null)
                    return defaultBalance;

                return leaveTransaction.Balance;
            }
        }
    }
}